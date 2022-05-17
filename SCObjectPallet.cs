using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDL2;
using XenoLib;

namespace XenoLib
{
    /// <summary>
    /// Basic types of objects in The Dwarven Forge
    /// </summary>
    public enum FORGETYPES { UNITS = 0, BUILDINGS, RESOURCES, DOODADS };
    /// <summary>
    /// Selects and returns a reference to objects in databases
    /// in The Dwarven Forge
    /// </summary>
    public class ObjectPallet
    {
        //protected
        protected SCRTSObject stamp;
        protected ScrollingList2 options;
        protected SimpleButton4 unitButton;
        protected SimpleButton4 buildingButton;
        protected SimpleButton4 resourceButton;
        protected SimpleButton4 doodadButton;
        protected RTSWorld world;
        protected Point2D topLeft;
        protected FORGETYPES objType;
        protected Rectangle pointerBox;

        //public
        /// <summary>
        /// ObjectPallet constructor
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="world">World reference</param>
        public ObjectPallet(int x, int y, RTSWorld world)
        {
            topLeft = new Point2D(x, y);
            this.world = world;
            stamp = null;
            options = new ScrollingList2(TextureBank.getTexture("down button 2"),
                TextureBank.getTexture("down button 2"), TextureBank.getTexture("up button 2"),
                TextureBank.getTexture("up button 2"), TextureBank.getTexture("blank"), 
                x, y + 32, 416, 576, 416, 32, 18);
            unitButton = new SimpleButton4(TextureBank.getTexture("create unit"),
                TextureBank.getTexture("create unit"), x, y, "units");
            unitButton.setSize(32, 32);
            buildingButton = new SimpleButton4(TextureBank.getTexture("create building"),
                TextureBank.getTexture("create building"), x + 32, y, "buildings");
            buildingButton.setSize(32, 32);
            resourceButton = new SimpleButton4(TextureBank.getTexture("create resource"),
                TextureBank.getTexture("create resource"), x + 64, y, "resources");
            resourceButton.setSize(32, 32);
            doodadButton = new SimpleButton4(TextureBank.getTexture("create doodad"),
                TextureBank.getTexture("create doodad"), x + 96, y, "doodads");
            doodadButton.setSize(32, 32);
            objType = FORGETYPES.UNITS;
            pointerBox = new Rectangle(0, 0, 2, 2);
        }
        /// <summary>
        /// Update ObjectPallet internal state
        /// </summary>
        /// <param name="pointer">SimplePointer reference</param>
        /// <param name="factionIndex">FactionIndex value</param>
        public void update(SimplePointer pointer, int factionIndex = 0)
        {
            if(unitButton.clicked() == true)
            {
                objType = FORGETYPES.UNITS;
                options.Clear();
                for(int i = 0; i < world.CommanderDB.at(factionIndex).UnitDB.Index; i++)
                {
                    options.addOption(world.CommanderDB.at(factionIndex).UnitDB.Names[i]);
                }
            }
            if (buildingButton.clicked() == true)
            {
                objType = FORGETYPES.BUILDINGS;
                options.Clear();
                for (int i = 0; i < world.CommanderDB.at(factionIndex).BuildingDB.Index; i++)
                {
                    options.addOption(world.CommanderDB.at(factionIndex).BuildingDB.Names[i]);
                }
            }
            if (resourceButton.clicked() == true)
            {
                objType = FORGETYPES.RESOURCES;
                options.Clear();
                for (int i = 0; i < world.ResourceDB.Index; i++)
                {
                    options.addOption(world.ResourceDB.Names[i]);
                }
            }
            if (doodadButton.clicked() == true)
            {
                objType = FORGETYPES.DOODADS;
                options.Clear();
                for (int i = 0; i < world.DoodadDB.Index; i++)
                {
                    options.addOption(world.DoodadDB.Names[i]);
                }
            }
            pointerBox.X = pointer.Tip.X;
            pointerBox.Y = pointer.Tip.Y;
            string choice = options.update(MouseHandler.getLeft(), pointerBox);
            if(choice != "" && choice != " ")
            {
                switch(objType)
                {
                    case FORGETYPES.UNITS:
                        stamp = world.CommanderDB.at(factionIndex).UnitDB.getData(choice);
                        break;
                    case FORGETYPES.BUILDINGS:
                        stamp = world.CommanderDB.at(factionIndex).BuildingDB.getData(choice);
                        break;
                    case FORGETYPES.RESOURCES:
                        stamp = world.ResourceDB.getData(choice);
                        break;
                    case FORGETYPES.DOODADS:
                        stamp = world.DoodadDB.getData(choice);
                        break;
                }
                unitButton.update();
                buildingButton.update();
                resourceButton.update();
            }
        }
        /// <summary>
        /// Draws ObjectPallet
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        public void draw(IntPtr renderer)
        {
            unitButton.draw(renderer);
            buildingButton.draw(renderer);
            resourceButton.draw(renderer);
            doodadButton.draw(renderer);
            options.draw(renderer, XENOCOLOURS.BLACK, 
                XENOCOLOURS.WHITE, 1f);
        }
        /// <summary>
        /// Stamp property
        /// </summary>
        public SCRTSObject Stamp
        {
            get { return stamp; }
        }
        /// <summary>
        /// ObjType property
        /// </summary>
        public FORGETYPES OT
        {
            get { return objType; }
            set { objType = value; }
        }
    }
}
