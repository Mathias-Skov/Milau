using Swed64;
using System.Numerics;

namespace Milau
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Initalize Swed library
                Swed swed = new Swed("cs2");

                // Get client module
                IntPtr client = swed.GetModuleBase("client.dll");

                // Initalize our GUI in a seperate thread
                Renderer renderer = new Renderer();
                renderer.screenSize = WindowUtils.GetGameWindowSize("cs2");
                Thread rendererThread = new Thread(new ThreadStart(renderer.Start().Wait));
                rendererThread.Start();

                // Get screen size
                Vector2 screenSize = renderer.screenSize;

                // Store entities
                List<Entity> entities = new List<Entity>();
                Entity localPlayer = new Entity();

                // Offsets from a2x CS2 dumper

                // offsets.cs
                int dwEntityList = 0x1A044C0;
                int dwViewMatrix = 0x1A6D260;
                int dwLocalPlayerPawn = 0x18580D0;

                // client.dll.cs
                int m_vOldOrigin = 0x1324;
                int m_iTeamNum = 0x3E3;
                int m_lifeState = 0x348;
                int m_hPlayerPawn = 0x824;
                int m_vecViewOffset = 0xCB0;

                Console.WriteLine("Milau loaded successfully!");

                // ESP loop
                while (true)
                {
                    // Clear entities list
                    entities.Clear();

                    // Get entity list
                    IntPtr entityList = swed.ReadPointer(client + dwEntityList);

                    // Make entry
                    IntPtr listEntry = swed.ReadPointer(entityList, 0x10);

                    // Get localplayer
                    IntPtr localPlayerPawn = swed.ReadPointer(client + dwLocalPlayerPawn);

                    // Get team so we can compare with other entities
                    localPlayer.team = swed.ReadInt(localPlayerPawn + m_iTeamNum);

                    // Loop through entities
                    for (int i = 0; i < 64; i++) // 64 pointers?
                    {
                        IntPtr currentController = swed.ReadPointer(listEntry, i * 0x78);

                        if (currentController == IntPtr.Zero) continue;

                        // Get pawn handle
                        int pawnHandle = swed.ReadInt(currentController, m_hPlayerPawn);
                        if (pawnHandle == 0) continue;

                        // Get current pawn, make second entry
                        IntPtr listEntry2 = swed.ReadPointer(entityList, 0x8 * ((pawnHandle & 0x7FFF) >> 9) + 0x10);
                        if (listEntry2 == IntPtr.Zero) continue;

                        // Get current pawn
                        IntPtr currentPawn = swed.ReadPointer(listEntry2, 0x78 * (pawnHandle & 0x1FF));
                        if (currentPawn == IntPtr.Zero) continue;

                        // Check if lifestate
                        int lifeState = swed.ReadInt(currentPawn + m_lifeState);
                        if (lifeState != 256) continue;

                        // Get matrix 
                        float[] viewMatrix = swed.ReadMatrix(client + dwViewMatrix);

                        // Poopulate entity
                        Entity entity = new Entity();

                        entity.team = swed.ReadInt(currentPawn + m_iTeamNum);
                        entity.position = swed.ReadVec(currentPawn, m_vOldOrigin);
                        entity.viewOffset = swed.ReadVec(currentPawn, m_vecViewOffset);
                        entity.position2D = Calculate.WorldToScreen(viewMatrix, entity.position, screenSize);
                        entity.viewPosition2D = Calculate.WorldToScreen(viewMatrix, Vector3.Add(entity.position, entity.viewOffset), screenSize);

                        entities.Add(entity);
                    }
                    // Update renderer data
                    renderer.UpdateLocalPlayer(localPlayer);
                    renderer.UpdateEntities(entities);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error! {ex.Message}");
                Console.WriteLine("Make sure CS2 is running before executing the program!");
            }
        }
    }
}   
