using System.IO;
using Cinemachine;
using HuyetNguyetDaiNgan.Combat;
using HuyetNguyetDaiNgan.Core;
using HuyetNguyetDaiNgan.Player;
using HuyetNguyetDaiNgan.UI;
using HuyetNguyetDaiNgan.World;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace HuyetNguyetDaiNgan.Editor
{
    public static class M4_SurvivalSceneSetupHelper
    {
        private const string ScenePath = "Assets/_Project/Scenes/Sandbox/Test_SurvivalCore.unity";
        private const string ConfigFolder = "Assets/_Project/Data/Config";
        private const string ConfigPath = ConfigFolder + "/SurvivalConfig_Default.asset";
        private const string ItemsFolder = "Assets/_Project/Data/Items";
        private const string WeaponAssetPath = ItemsFolder + "/Weapon_IronSword_T1.asset";
        private const string MaterialsFolder = "Assets/_Project/Art/Materials";

        [MenuItem("Huyết Nguyệt Đại Ngàn/M4: Setup Survival Core Scene", false, 50)]
        public static void SetupM4SurvivalScene()
        {
            EnsureDirectories();

            Shader urpLit = Shader.Find("Universal Render Pipeline/Lit");
            if (urpLit == null) urpLit = Shader.Find("Standard");

            // 1. Materials
            Material grassMat = GetOrCreateMaterial(MaterialsFolder + "/M_Survival_Grass.mat", new Color(0.28f, 0.42f, 0.22f, 1f), urpLit);
            Material trunkMat = GetOrCreateMaterial(MaterialsFolder + "/M_Wood_Trunk.mat", new Color(0.45f, 0.28f, 0.15f, 1f), urpLit);
            Material foliageMat = GetOrCreateMaterial(MaterialsFolder + "/M_Wood_Foliage.mat", new Color(0.18f, 0.48f, 0.22f, 1f), urpLit);
            Material stoneMat = GetOrCreateMaterial(MaterialsFolder + "/M_Stone_Rock.mat", new Color(0.55f, 0.58f, 0.62f, 1f), urpLit);
            Material bushMat = GetOrCreateMaterial(MaterialsFolder + "/M_Berry_Bush.mat", new Color(0.25f, 0.55f, 0.25f, 1f), urpLit);
            Material berryMat = GetOrCreateMaterial(MaterialsFolder + "/M_Berry_Fruit.mat", new Color(0.92f, 0.20f, 0.20f, 1f), urpLit);
            Material stumpMat = GetOrCreateMaterial(MaterialsFolder + "/M_Wood_Stump.mat", new Color(0.38f, 0.25f, 0.14f, 1f), urpLit);
            Material wallMat = GetOrCreateMaterial(MaterialsFolder + "/M_SurvivalWall_URP.mat", new Color(0.32f, 0.35f, 0.30f, 1f), urpLit);

            // 2. Load or Create ScriptableObjects
            SurvivalConfigSO survivalConfig = GetOrCreateSurvivalConfig();
            WeaponDataSO swordData = GetOrCreateWeaponData();

            // 3. New Scene
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            // 4. Lighting & Environment
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.40f, 0.42f, 0.45f, 1f);

            var lightObj = new GameObject("Directional Light (Sun)");
            var sunLight = lightObj.AddComponent<Light>();
            sunLight.type = LightType.Directional;
            sunLight.color = new Color(1f, 0.96f, 0.88f);
            sunLight.intensity = 1.35f;
            sunLight.shadows = LightShadows.Soft;
            lightObj.transform.rotation = Quaternion.Euler(50f, -35f, 0f);

            // 5. Arena Ground (50m x 50m)
            var groundObj = GameObject.CreatePrimitive(PrimitiveType.Plane);
            groundObj.name = "Survival_Forest_Floor";
            groundObj.transform.position = Vector3.zero;
            groundObj.transform.localScale = new Vector3(5f, 1f, 5f);
            groundObj.GetComponent<Renderer>().sharedMaterial = grassMat;

            // Boundary Walls
            CreateBoundaryWalls(groundObj, wallMat);

            // 6. Spawn Resource Nodes
            GameObject environmentRoot = new GameObject("Environment_Resources");

            // Rừng cây (10 cây xanh mát rải rác theo cụm)
            Vector3[] treePositions = new Vector3[]
            {
                new Vector3(-6f, 0f, 4f),
                new Vector3(-9f, 0f, 6f),
                new Vector3(-4f, 0f, 8f),
                new Vector3(-8f, 0f, 12f),
                new Vector3(7f, 0f, 5f),
                new Vector3(10f, 0f, 7f),
                new Vector3(5f, 0f, 10f),
                new Vector3(8f, 0f, 13f),
                new Vector3(-12f, 0f, -5f),
                new Vector3(-15f, 0f, -8f)
            };
            for (int i = 0; i < treePositions.Length; i++)
            {
                CreateTreeResourceNode(treePositions[i], i + 1, trunkMat, foliageMat, stumpMat, environmentRoot.transform);
            }

            // Mỏ đá (6 tảng đá xám)
            Vector3[] rockPositions = new Vector3[]
            {
                new Vector3(6f, 0f, -4f),
                new Vector3(9f, 0f, -7f),
                new Vector3(4f, 0f, -8f),
                new Vector3(12f, 0f, -3f),
                new Vector3(-7f, 0f, -10f),
                new Vector3(-10f, 0f, -13f)
            };
            for (int i = 0; i < rockPositions.Length; i++)
            {
                CreateRockResourceNode(rockPositions[i], i + 1, stoneMat, environmentRoot.transform);
            }

            // Bụi quả dại (5 bụi quả mọng đỏ)
            Vector3[] bushPositions = new Vector3[]
            {
                new Vector3(0f, 0f, 6f),
                new Vector3(-2f, 0f, -4f),
                new Vector3(2f, 0f, -5f),
                new Vector3(-5f, 0f, -2f),
                new Vector3(3f, 0f, 3f)
            };
            for (int i = 0; i < bushPositions.Length; i++)
            {
                CreateBerryBushNode(bushPositions[i], i + 1, bushMat, berryMat, environmentRoot.transform);
            }

            // 7. Spawn Player with Full Survival & Combat
            GameObject player = CreatePlayerWithSurvival(swordData, survivalConfig);
            player.transform.position = new Vector3(0f, 0.1f, 0f);

            // 8. Camera with Cinemachine
            var camObj = new GameObject("Main Camera");
            camObj.tag = "MainCamera";
            var cam = camObj.AddComponent<Camera>();
            cam.fieldOfView = 40f;
            cam.nearClipPlane = 0.3f;
            cam.farClipPlane = 200f;
            camObj.AddComponent<UniversalAdditionalCameraData>();
            camObj.AddComponent<CinemachineBrain>();

            var vcamObj = new GameObject("CM_Survival_Vcam");
            var vcam = vcamObj.AddComponent<CinemachineVirtualCamera>();
            vcam.Follow = player.transform;
            var transposer = vcam.AddCinemachineComponent<CinemachineTransposer>();
            transposer.m_BindingMode = CinemachineTransposer.BindingMode.WorldSpace;
            transposer.m_FollowOffset = new Vector3(-14f, 16f, -14f);
            transposer.m_XDamping = 0.5f;
            transposer.m_YDamping = 0.5f;
            transposer.m_ZDamping = 0.5f;
            vcam.transform.rotation = Quaternion.Euler(45f, 45f, 0f);

            // 9. Survival HUD & Runtime Display
            var hudObj = new GameObject("Survival_HUD");
            hudObj.AddComponent<SurvivalHUD>();

            // 10. Save Scene
            EditorSceneManager.SaveScene(scene, ScenePath);
            AddSceneToBuildSettings(ScenePath);

            AssetDatabase.Refresh();
            Debug.Log($"[Huyết Nguyệt M4] Đã tạo thành công Scene '{ScenePath}' với Rừng cây, Mỏ đá, Bụi quả và Survival HUD!");
        }

        private static GameObject CreatePlayerWithSurvival(WeaponDataSO weapon, SurvivalConfigSO config)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_Project/Prefabs/Player/Player_Prototype.prefab");
            GameObject player;

            if (prefab != null)
            {
                player = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            }
            else
            {
                player = new GameObject("Player");
                var cc = player.AddComponent<CharacterController>();
                cc.height = 1.8f;
                cc.radius = 0.4f;
                cc.center = new Vector3(0f, 0.9f, 0f);
                player.AddComponent<PlayerInputHandler>();
                player.AddComponent<PlayerMovementController>();

                var vis = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                vis.name = "PlayerVisual";
                vis.transform.SetParent(player.transform);
                vis.transform.localPosition = new Vector3(0f, 0.9f, 0f);
                Object.DestroyImmediate(vis.GetComponent<Collider>());
            }

            player.name = "Player";
            player.tag = "Player";

            // Health
            var health = player.GetComponent<HealthComponent>();
            if (health == null) health = player.AddComponent<HealthComponent>();
            health.SetMaxHealth(100f, true);

            // Hit feedback
            if (player.GetComponent<HitFeedbackFlash>() == null)
            {
                player.AddComponent<HitFeedbackFlash>();
            }

            // Stats Component
            var stats = player.GetComponent<PlayerStatsComponent>();
            if (stats == null) stats = player.AddComponent<PlayerStatsComponent>();
            stats.SetConfig(config);

            // Combat Controller
            var combat = player.GetComponent<PlayerCombatController>();
            if (combat == null) combat = player.AddComponent<PlayerCombatController>();
            combat.EquipWeapon(weapon);

            return player;
        }

        private static void CreateTreeResourceNode(Vector3 pos, int id, Material trunkMat, Material foliageMat, Material stumpMat, Transform parent)
        {
            var treeObj = new GameObject($"Tree_Node_{id}");
            treeObj.transform.SetParent(parent);
            treeObj.transform.position = pos;

            // Box/Capsule Collider
            var col = treeObj.AddComponent<CapsuleCollider>();
            col.height = 3.5f;
            col.radius = 0.5f;
            col.center = new Vector3(0f, 1.75f, 0f);

            // Visual Root (để lắc wobble khi trúng đòn)
            var visualRoot = new GameObject("Visual");
            visualRoot.transform.SetParent(treeObj.transform);
            visualRoot.transform.localPosition = Vector3.zero;

            // Trunk (Thân cây gỗ)
            var trunk = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            trunk.name = "Trunk";
            trunk.transform.SetParent(visualRoot.transform);
            trunk.transform.localPosition = new Vector3(0f, 1.25f, 0f);
            trunk.transform.localScale = new Vector3(0.5f, 1.25f, 0.5f);
            trunk.GetComponent<Renderer>().sharedMaterial = trunkMat;
            Object.DestroyImmediate(trunk.GetComponent<Collider>());

            // Foliage (Tán lá cây)
            var foliage = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            foliage.name = "Foliage";
            foliage.transform.SetParent(visualRoot.transform);
            foliage.transform.localPosition = new Vector3(0f, 2.7f, 0f);
            foliage.transform.localScale = new Vector3(2.2f, 1.8f, 2.2f);
            foliage.GetComponent<Renderer>().sharedMaterial = foliageMat;
            Object.DestroyImmediate(foliage.GetComponent<Collider>());

            // Depleted Stump (Gốc cây khi bị chặt hết)
            var stump = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            stump.name = "DepletedStump";
            stump.transform.SetParent(treeObj.transform);
            stump.transform.localPosition = new Vector3(0f, 0.15f, 0f);
            stump.transform.localScale = new Vector3(0.55f, 0.15f, 0.55f);
            stump.GetComponent<Renderer>().sharedMaterial = stumpMat;
            Object.DestroyImmediate(stump.GetComponent<Collider>());
            stump.SetActive(false);

            // Component ResourceNode
            var node = treeObj.AddComponent<ResourceNode>();
            SetPrivateField(node, "resourceType", ResourceType.Wood);
            SetPrivateField(node, "hitsToDeplete", 3);
            SetPrivateField(node, "respawnTime", 20f);
            SetPrivateField(node, "intactVisual", visualRoot.transform);
            SetPrivateField(node, "depletedVisual", stump.transform);
            SetPrivateField(node, "nodeCollider", col);
        }

        private static void CreateRockResourceNode(Vector3 pos, int id, Material stoneMat, Transform parent)
        {
            var rockObj = new GameObject($"Rock_Node_{id}");
            rockObj.transform.SetParent(parent);
            rockObj.transform.position = pos;

            var col = rockObj.AddComponent<SphereCollider>();
            col.radius = 0.9f;
            col.center = new Vector3(0f, 0.7f, 0f);

            var visualRoot = new GameObject("Visual");
            visualRoot.transform.SetParent(rockObj.transform);
            visualRoot.transform.localPosition = Vector3.zero;

            // Main Rock Mesh
            var mainRock = GameObject.CreatePrimitive(PrimitiveType.Cube);
            mainRock.name = "Main_Boulder";
            mainRock.transform.SetParent(visualRoot.transform);
            mainRock.transform.localPosition = new Vector3(0f, 0.65f, 0f);
            mainRock.transform.localScale = new Vector3(1.3f, 1.1f, 1.3f);
            mainRock.transform.rotation = Quaternion.Euler(UnityEngine.Random.Range(10f, 25f), UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(10f, 25f));
            mainRock.GetComponent<Renderer>().sharedMaterial = stoneMat;
            Object.DestroyImmediate(mainRock.GetComponent<Collider>());

            // Depleted Flat Rock Base
            var depletedRock = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            depletedRock.name = "DepletedRock";
            depletedRock.transform.SetParent(rockObj.transform);
            depletedRock.transform.localPosition = new Vector3(0f, 0.1f, 0f);
            depletedRock.transform.localScale = new Vector3(1.1f, 0.1f, 1.1f);
            depletedRock.GetComponent<Renderer>().sharedMaterial = stoneMat;
            Object.DestroyImmediate(depletedRock.GetComponent<Collider>());
            depletedRock.SetActive(false);

            var node = rockObj.AddComponent<ResourceNode>();
            SetPrivateField(node, "resourceType", ResourceType.Stone);
            SetPrivateField(node, "hitsToDeplete", 4);
            SetPrivateField(node, "respawnTime", 25f);
            SetPrivateField(node, "intactVisual", visualRoot.transform);
            SetPrivateField(node, "depletedVisual", depletedRock.transform);
            SetPrivateField(node, "nodeCollider", col);
        }

        private static void CreateBerryBushNode(Vector3 pos, int id, Material bushMat, Material berryMat, Transform parent)
        {
            var bushObj = new GameObject($"Berry_Bush_{id}");
            bushObj.transform.SetParent(parent);
            bushObj.transform.position = pos;

            var col = bushObj.AddComponent<SphereCollider>();
            col.radius = 0.7f;
            col.center = new Vector3(0f, 0.5f, 0f);

            var visualRoot = new GameObject("Visual");
            visualRoot.transform.SetParent(bushObj.transform);
            visualRoot.transform.localPosition = Vector3.zero;

            // Green foliage
            var bush = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            bush.name = "BushFoliage";
            bush.transform.SetParent(visualRoot.transform);
            bush.transform.localPosition = new Vector3(0f, 0.45f, 0f);
            bush.transform.localScale = new Vector3(1.1f, 0.8f, 1.1f);
            bush.GetComponent<Renderer>().sharedMaterial = bushMat;
            Object.DestroyImmediate(bush.GetComponent<Collider>());

            // Red berries
            Vector3[] berryOffsets = new Vector3[]
            {
                new Vector3(0.35f, 0.6f, 0.2f),
                new Vector3(-0.3f, 0.55f, 0.3f),
                new Vector3(0.1f, 0.7f, -0.35f)
            };
            foreach (var offset in berryOffsets)
            {
                var berry = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                berry.name = "Berry";
                berry.transform.SetParent(visualRoot.transform);
                berry.transform.localPosition = offset;
                berry.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                berry.GetComponent<Renderer>().sharedMaterial = berryMat;
                Object.DestroyImmediate(berry.GetComponent<Collider>());
            }

            var node = bushObj.AddComponent<ResourceNode>();
            SetPrivateField(node, "resourceType", ResourceType.Food);
            SetPrivateField(node, "hitsToDeplete", 1);
            SetPrivateField(node, "respawnTime", 15f);
            SetPrivateField(node, "intactVisual", visualRoot.transform);
            SetPrivateField(node, "nodeCollider", col);
        }

        private static void CreateBoundaryWalls(GameObject parent, Material mat)
        {
            float size = 25f;
            float thickness = 1f;
            float height = 2.5f;

            CreateWall(parent, "Wall_North", new Vector3(0f, height * 0.5f, size), new Vector3(size * 2, height, thickness), mat);
            CreateWall(parent, "Wall_South", new Vector3(0f, height * 0.5f, -size), new Vector3(size * 2, height, thickness), mat);
            CreateWall(parent, "Wall_East", new Vector3(size, height * 0.5f, 0f), new Vector3(thickness, height, size * 2), mat);
            CreateWall(parent, "Wall_West", new Vector3(-size, height * 0.5f, 0f), new Vector3(thickness, height, size * 2), mat);
        }

        private static void CreateWall(GameObject parent, string name, Vector3 pos, Vector3 scale, Material mat)
        {
            var wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.name = name;
            wall.transform.SetParent(parent.transform);
            wall.transform.position = pos;
            wall.transform.localScale = scale;
            wall.GetComponent<Renderer>().sharedMaterial = mat;
        }

        private static Material GetOrCreateMaterial(string path, Color color, Shader shader)
        {
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (mat == null)
            {
                mat = new Material(shader) { color = color };
                AssetDatabase.CreateAsset(mat, path);
            }
            return mat;
        }

        private static SurvivalConfigSO GetOrCreateSurvivalConfig()
        {
            SurvivalConfigSO config = AssetDatabase.LoadAssetAtPath<SurvivalConfigSO>(ConfigPath);
            if (config == null)
            {
                config = ScriptableObject.CreateInstance<SurvivalConfigSO>();
                config.maxHunger = 100f;
                config.hungerDrainRate = 0.5f;
                config.wellFedThreshold = 80f;
                config.wellFedHealthRegenRate = 1.5f;
                config.starvationDamageRate = 3.0f;
                config.maxStamina = 100f;
                config.sprintStaminaDrainRate = 14f;
                config.attackStaminaCost = 15f;
                config.staminaRegenRate = 22f;
                config.staminaRegenDelay = 0.8f;
                config.maxMana = 100f;
                config.manaRegenRate = 5f;

                AssetDatabase.CreateAsset(config, ConfigPath);
                AssetDatabase.SaveAssets();
            }
            return config;
        }

        private static WeaponDataSO GetOrCreateWeaponData()
        {
            WeaponDataSO weapon = AssetDatabase.LoadAssetAtPath<WeaponDataSO>(WeaponAssetPath);
            if (weapon == null)
            {
                weapon = ScriptableObject.CreateInstance<WeaponDataSO>();
                weapon.weaponId = "weapon_sword_iron_t1";
                weapon.weaponName = "Trường Kiếm Sắt Ấp Làng";
                weapon.category = WeaponCategory.Sword;
                weapon.damageType = DamageType.Physical;
                weapon.baseDamage = 25f;
                weapon.attackRange = 2.5f;
                weapon.attackAngle = 110f;
                weapon.attackCooldown = 0.45f;
                weapon.knockbackForce = 3.0f;

                AssetDatabase.CreateAsset(weapon, WeaponAssetPath);
                AssetDatabase.SaveAssets();
            }
            return weapon;
        }

        private static void EnsureDirectories()
        {
            if (!AssetDatabase.IsValidFolder("Assets/_Project/Data"))
            {
                AssetDatabase.CreateFolder("Assets/_Project", "Data");
            }
            if (!AssetDatabase.IsValidFolder("Assets/_Project/Data/Config"))
            {
                AssetDatabase.CreateFolder("Assets/_Project/Data", "Config");
            }
            if (!AssetDatabase.IsValidFolder("Assets/_Project/Data/Items"))
            {
                AssetDatabase.CreateFolder("Assets/_Project/Data", "Items");
            }
            if (!AssetDatabase.IsValidFolder("Assets/_Project/Art/Materials"))
            {
                if (!AssetDatabase.IsValidFolder("Assets/_Project/Art"))
                {
                    AssetDatabase.CreateFolder("Assets/_Project", "Art");
                }
                AssetDatabase.CreateFolder("Assets/_Project/Art", "Materials");
            }
        }

        private static void SetPrivateField(object target, string fieldName, object value)
        {
            var field = target.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field?.SetValue(target, value);
        }

        private static void AddSceneToBuildSettings(string path)
        {
            var scenes = EditorBuildSettings.scenes;
            foreach (var s in scenes)
            {
                if (s.path == path) return;
            }
            var newScenes = new EditorBuildSettingsScene[scenes.Length + 1];
            scenes.CopyTo(newScenes, 0);
            newScenes[newScenes.Length - 1] = new EditorBuildSettingsScene(path, true);
            EditorBuildSettings.scenes = newScenes;
        }
    }
}

