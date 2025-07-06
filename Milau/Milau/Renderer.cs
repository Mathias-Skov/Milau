using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ClickableTransparentOverlay;
using ImGuiNET;

namespace Milau
{
    public class Renderer : Overlay
    {
        // Render variables
        public Vector2 screenSize = new Vector2(3840, 2160); // Default screen size

        // Entities copy by using more thread safe methods.
        private ConcurrentQueue<Entity> entities = new ConcurrentQueue<Entity>();
        private Entity localPlayer = new Entity();
        private readonly object entityLock = new object();

        // GUI elements
        private bool enableESP = true;
        private bool drawLines = false;

        private Vector4 enemyColor = new Vector4(1, 0, 0, 1); // RGBA format for red color
        private Vector4 teamColor = new Vector4(0, 0, 1, 1); // RGBA format for blue color

        // Draw list
        ImDrawListPtr drawList;


        protected override void Render()
        {
            // Draw our GUI
            ImGui.Begin("Milau");
            ImGui.Text("A CS2 cheat");

            // ESP part
            ImGui.Checkbox("Enable ESP", ref enableESP);
            ImGui.Checkbox("Draw lines", ref drawLines);

            // Enemy color
            if (ImGui.CollapsingHeader("Enemey color"))
                ImGui.ColorPicker4("##enemycolor", ref enemyColor);

            // Team color
            if (ImGui.CollapsingHeader("Team color"))
                ImGui.ColorPicker4("##teamcolor", ref teamColor);

            // Draw overlay
            DrawOverlay(screenSize);
            drawList = ImGui.GetWindowDrawList();

            // Draw stuff
            if (enableESP)
            {
                foreach (var entity in entities)
                {
                    if (EntityOnScreen(entity))
                    {
                        DrawBox(entity);
                        if (drawLines)
                            DrawLine(entity);
                    }
                }
            }

            // Misc
            ImGui.End();
        }

        // Check position
        bool EntityOnScreen(Entity entity)
        {
            if (entity.position2D.X > 0 && entity.position2D.X < screenSize.X && entity.position2D.Y > 0 && entity.position2D.Y < screenSize.Y)
            {
                return true; // Entity is on screen
            }
            return false; // Entity is off screen
        }

        // Drawing methods
        private void DrawBox(Entity entity)
        {
            // Calculate entity box height
            float entityHeight = entity.position2D.Y - entity.viewPosition2D.Y;
            
            // Calculate box dimensions
            Vector2 rectTop = new Vector2(entity.viewPosition2D.X - entityHeight / 3, entity.viewPosition2D.Y);
            Vector2 rectBottom = new Vector2(entity.position2D.X + entityHeight / 3, entity.position2D.Y);

            // Get correct color based on team
            Vector4 boxColor = localPlayer.team == entity.team ? teamColor : enemyColor;

            drawList.AddRect(rectTop, rectBottom, ImGui.ColorConvertFloat4ToU32(boxColor));
        }
        private void DrawLine(Entity entity)
        {
            // Get correct color based on team
            Vector4 lineColor = localPlayer.team == entity.team ? teamColor : enemyColor;

            // Draw line
            drawList.AddLine(new Vector2(screenSize.X / 2, screenSize.Y), entity.position2D, ImGui.ColorConvertFloat4ToU32(lineColor));
        }

        // Transfer entity methods
        public void UpdateEntities(IEnumerable<Entity> newEntities) // Update our entities
        {
            entities = new ConcurrentQueue<Entity>(newEntities);
        }

        public void UpdateLocalPlayer(Entity newEntity) // Update localplayer
        {
            lock (entityLock)
            {
                localPlayer = newEntity;
            }
        }

        void DrawOverlay(Vector2 screenSize)
        {
            ImGui.SetNextWindowSize(screenSize);
            ImGui.SetNextWindowPos(new Vector2(0, 0));
            ImGui.Begin("overlay", ImGuiWindowFlags.NoDecoration
                | ImGuiWindowFlags.NoBackground
                | ImGuiWindowFlags.NoBringToFrontOnFocus
                | ImGuiWindowFlags.NoMove
                | ImGuiWindowFlags.NoInputs
                | ImGuiWindowFlags.NoCollapse
                | ImGuiWindowFlags.NoScrollbar
                | ImGuiWindowFlags.NoScrollWithMouse
            );
        }
    }
}
