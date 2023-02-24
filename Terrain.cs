using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste_Forms_Edition
{
    class Terrain
    {
        public PictureBox pb;

        public Terrain(int posX, int posY, int width, int height, string tag, Color color, bool hasTexture, Image texture, Panel panel)
        {
            pb = new PictureBox
            {
                Left = posX,
                Top = posY,
                Width = width,
                Height = height,
                Tag = tag,
                BackColor = color,
                BackgroundImage = hasTexture ? texture : null,
                BackgroundImageLayout = ImageLayout.Stretch
            };
            panel.Controls.Add(pb);
            pb.BringToFront();
        }
    }
}
