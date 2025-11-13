using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using System.IO;
using System.Reflection;
using UnityEngine.U2D;
using VehicleFramework.VehicleBuilding;
using VehicleFramework.VehicleTypes;
using VehicleFramework.Engines;
using VehicleFramework.Interfaces;

namespace Atrama
{
    public class Atrama : Submarine, INavigationLights
    {
        public static GameObject model = null;
        public static UnityEngine.Sprite pingSprite = null;
        public static UnityEngine.Sprite crafterSprite = null;
        public static void GetAssets()
        {
            // load the asset bundle
            string modPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(modPath, "assets/atrama"));
            if (myLoadedAssetBundle == null)
            {
                VehicleFramework.Logger.Error("Atrama Error: Failed to load AssetBundle!");
                return;
            }

            System.Object[] arr = myLoadedAssetBundle.LoadAllAssets();
            foreach (System.Object obj in arr)
            {
                if (obj.ToString().Contains("SpriteAtlas"))
                {
                    SpriteAtlas thisAtlas = (SpriteAtlas)obj;
                    pingSprite = thisAtlas.GetSprite("AtramaHudPing");
                    crafterSprite = thisAtlas.GetSprite("CrafterSprite");
                }
                else if (obj.ToString().Contains("Atrama"))
                {
                    model = (GameObject)obj;
                }
            }
        }
        public override Dictionary<TechType, int> Recipe
        {
            get
            {
                Dictionary<TechType, int> recipe = new Dictionary<TechType, int>
                {
                    { TechType.TitaniumIngot, 1 },
                    { TechType.PlasteelIngot, 1 },
                    { TechType.Lubricant, 1 },
                    { TechType.AdvancedWiringKit, 1 },
                    { TechType.Lead, 2 },
                    { TechType.EnameledGlass, 2 }
                };
                return recipe;
            }
        }
        public static IEnumerator Register()
        {
            GetAssets();
            Submarine atrama = model.EnsureComponent<Atrama>() as Submarine;
            yield return UWE.CoroutineHost.StartCoroutine(VehicleFramework.Admin.VehicleRegistrar.RegisterVehicle(atrama));
        }
        public override string vehicleDefaultName
        {
            get
            {
                Language main = Language.main;
                if (!(main != null))
                {
                    return "ATRAMA";
                }
                return main.Get("AtramaDefaultName");
            }
        }
        public override string Description
        {
            get
            {
                return "A submarine built for construction. Its great top speed is second only to its enormous storage capacity.";
            }
        }
        public override void Awake()
        {
            // Give the Odyssey a new name and make sure we track it well.
            string newName = "ATR-" + Mathf.RoundToInt(UnityEngine.Random.value * 10000).ToString();
            SetName(newName);
            base.Awake();
        }
        public override string EncyclopediaEntry
        {
            get
            {
                /*
                 * The Formula:
                 * 2 or 3 sentence blurb
                 * Features
                 * Advice
                 * Ratings
                 * Kek
                 */
                string ency = "The Atrama is a submarine purpose built for Construction. ";
                ency += "Its signature arms (in development) are what earned it its Lithuanian name. \n";
                ency += "\nIt features:\n";
                ency += "- Two arms which have several different attachments (in development). \n";
                ency += "- Ample storage capacity, which can be further expanded by upgrades. \n";
                ency += "- A signature autopilot which can automatically level out the vessel. \n";
                ency += "\nRatings:\n";
                ency += "- Top Speed: 15m/s \n";
                ency += "- Acceleration: 3m/s/s \n";
                ency += "- Distance per Power Cell: 7.5km \n";
                ency += "- Crush Depth: 900 \n";
                ency += "- Upgrade Slots: 6 \n";
                ency += "- Dimensions: 7.5m x 4m x 14.5m \n";
                ency += "- Persons: 1-2 \n";
                ency += "\n\"Pass on the drama- just build the Atrama.\" ";
                return ency;
            }
        }
        public override List<VehicleBattery> Batteries
        {
            get
            {
                List<Transform> batteryTransforms = new List<Transform>()
                {
                    transform.Find("model/Mechanical-Panel/BatteryInputs/1"),
                    transform.Find("model/Mechanical-Panel/BatteryInputs/2"),
                    transform.Find("model/Mechanical-Panel/BatteryInputs/3"),
                    transform.Find("model/Mechanical-Panel/BatteryInputs/4"),
                };
                VehicleBattery CreateStorage(Transform tr)
                {
                    return new VehicleBattery
                    {
                        BatterySlot = tr.gameObject
                    };
                }
                return batteryTransforms.Select(CreateStorage).ToList();
            }
        }
        public override GameObject VehicleModel
        {
            get
            {
                return model;
            }
        }
        public override List<VehicleHatchStruct> Hatches
        {
            get
            {
                var list = new List<VehicleHatchStruct>();
                VehicleHatchStruct vhs = new VehicleHatchStruct();
                Transform mainHatch = transform.Find("model/Hatch");
                vhs.Hatch = mainHatch.gameObject;
                vhs.EntryLocation = mainHatch.Find("Entry");
                vhs.ExitLocation = mainHatch.Find("Exit");
                vhs.SurfaceExitLocation = mainHatch.Find("SurfaceExit");
                list.Add(vhs);
                return list;
            }
        }
        public override List<VehicleFloodLight> HeadLights
        {
            get
            {
                var list = new List<VehicleFloodLight>();

                VehicleFloodLight leftLight = new VehicleFloodLight
                {
                    Light = transform.Find("lights_parent/HeadLights/LeftLight").gameObject,
                    Angle = 60,
                    Color = Color.white,
                    Intensity = 1.5f,
                    Range = 120f
                };
                list.Add(leftLight);

                VehicleFloodLight rightLight = new VehicleFloodLight
                {
                    Light = transform.Find("lights_parent/HeadLights/RightLight").gameObject,
                    Angle = 60,
                    Color = Color.white,
                    Intensity = 1.5f,
                    Range = 120f
                };
                list.Add(rightLight);

                return list;
            }
        }
        public override List<VehicleFloodLight> FloodLights
        {
            get
            {
                var list = new List<VehicleFloodLight>();

                VehicleFloodLight mainFlood = new VehicleFloodLight
                {
                    Light = transform.Find("lights_parent/FloodLights/main").gameObject,
                    Angle = 120,
                    Color = Color.white,
                    Intensity = 1f,
                    Range = 100f
                };
                list.Add(mainFlood);


                VehicleFloodLight portFlood = new VehicleFloodLight
                {
                    Light = transform.Find("lights_parent/FloodLights/port").gameObject,
                    Angle = 90,
                    Color = Color.white,
                    Intensity = 1,
                    Range = 100f
                };
                list.Add(portFlood);


                VehicleFloodLight starboardFlood = new VehicleFloodLight
                {
                    Light = transform.Find("lights_parent/FloodLights/starboard").gameObject,
                    Angle = 90,
                    Color = Color.white,
                    Intensity = 1f,
                    Range = 100f
                };
                list.Add(starboardFlood);

                return list;
            }
        }
        public override VehiclePilotSeat PilotSeat
        {
            get
            {
                Transform mainSeat = transform.Find("model/PilotSeat");
                VehiclePilotSeat vps = new VehiclePilotSeat
                {
                    Seat = mainSeat.gameObject,
                    SitLocation = mainSeat.Find("SitLocation").gameObject,
                    ExitLocation = mainSeat.Find("ExitLocation"),
                    LeftHandLocation = mainSeat,
                    RightHandLocation = mainSeat
                };
                return vps;
            }
        }
        public override List<VehicleStorage> InnateStorages
        {
            get
            {
                List<Transform> storageTransforms = new List<Transform>()
                {
                    transform.Find("model/InnateStorage/LeftStorage1"),
                    transform.Find("model/InnateStorage/LeftStorage2"),
                    transform.Find("model/InnateStorage/RightStorage2"),
                    transform.Find("model/InnateStorage/RightStorage1"),
                };
                VehicleStorage CreateStorage(Transform tr)
                {
                    return new VehicleStorage
                    {
                        Container = tr.gameObject,
                        Height = 8,
                        Width = 8
                    };
                }
                return storageTransforms.Select(CreateStorage).ToList();
            }
        }
        /*
        public override List<VehicleStorage> ModularStorages
        {
            get
            {
                var list = new List<VehicleStorage>();
                for (int i = 1; i < 7; i++)
                {
                    VehicleStorage thisVS = new VehicleStorage();
                    Transform thisStorage = transform.Find("model/ModularStorage/StorageModule" + i.ToString());
                    thisVS.Container = thisStorage.gameObject;
                    thisVS.Height = 4;
                    thisVS.Width = 4;
                    list.Add(thisVS);
                }
                return list;
            }
        }
        */
        public override List<VehicleUpgrades> Upgrades
        {
            get
            {
                return new List<VehicleUpgrades>()
                {
                    new VehicleUpgrades
                    {
                        Interface = transform.Find("model/Mechanical-Panel/Upgrades-Panel").gameObject
                    }
                };
            }
        }
        public override GameObject ControlPanel => transform.Find("Control-Panel").gameObject;
        public override GameObject ColorPicker => null;
        public override GameObject Fabricator => transform.Find("Fabricator-Location").gameObject;
        public override BoxCollider BoundingBoxCollider => transform.Find("model/BoundingBox").GetComponent<BoxCollider>();
        public override List<GameObject> TetherSources
        {
            get
            {
                var list = new List<GameObject>();
                foreach (Transform child in transform.Find("model/TetherSources"))
                {
                    list.Add(child.gameObject);
                }
                return list;
            }
        }
        public override List<GameObject> WaterClipProxies
        {
            get
            {
                var list = new List<GameObject>();
                foreach (Transform child in transform.Find("model/WaterClipProxies"))
                {
                    list.Add(child.gameObject);
                }
                return list;
            }
        }
        public override List<GameObject> CanopyWindows
        {
            get
            {
                return new List<GameObject>
                {
                    transform.Find("model/Canopy").gameObject
                };
            }
        }
        List<GameObject> INavigationLights.NavigationPortLights()
        {
            var list = new List<GameObject>();
            foreach (Transform child in transform.Find("lights_parent/NavigationLights/PortLights"))
            {
                list.Add(child.gameObject);
            }
            return list;
        }
        List<GameObject> INavigationLights.NavigationStarboardLights()
        {
            var list = new List<GameObject>();
            foreach (Transform child in transform.Find("lights_parent/NavigationLights/StarboardLights"))
            {
                list.Add(child.gameObject);
            }
            return list;
        }
        List<GameObject> INavigationLights.NavigationPositionLights()
        {
            var list = new List<GameObject>();
            foreach (Transform child in transform.Find("lights_parent/NavigationLights/PositionLights"))
            {
                list.Add(child.gameObject);
            }
            return list;
        }
        List<GameObject> INavigationLights.NavigationWhiteStrobeLights()
        {
            var list = new List<GameObject>();
            foreach (Transform child in transform.Find("lights_parent/NavigationLights/WhiteStrobes"))
            {
                list.Add(child.gameObject);
            }
            return list;
        }
        List<GameObject> INavigationLights.NavigationRedStrobeLights()
        {
            var list = new List<GameObject>();
            foreach (Transform child in transform.Find("lights_parent/NavigationLights/RedStrobes"))
            {
                list.Add(child.gameObject);
            }
            return list;
        }
        public override GameObject[] CollisionModel => new GameObject[] { transform.Find("model/CollisionModel").gameObject };
        public override VFEngine VFEngine => gameObject.EnsureComponent<AtramaEngine>();
        public override UnityEngine.Sprite PingSprite => pingSprite;
        public override int BaseCrushDepth => 900;
        public override int MaxHealth => 1000;
        public override int Mass => 4250;
        public override int NumModules => 8;
        public override bool HasArms => false;
        public override UnityEngine.Sprite CraftingSprite => crafterSprite;
        public override List<Transform> LavaLarvaAttachPoints
        {
            get
            {
                var list = new List<Transform>();
                foreach (Transform child in transform.Find("LLAttachPoints"))
                {
                    list.Add(child);
                }
                return list;
            }
        }
        public override List<Light> InteriorLights => new List<Light>() { transform.Find("lights_parent/WallLamp/light").gameObject.GetComponent<Light>() };
    }
}