using System.IO;
using Cinemachine;
using HuyetNguyetDaiNgan.AI;
using HuyetNguyetDaiNgan.Combat;
using HuyetNguyetDaiNgan.Player;
using HuyetNguyetDaiNgan.UI;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace HuyetNguyetDaiNgan.Editor
{
    public static class M3_CombatSceneSetupHelper
    {
        private const string ScenePath = "Assets/_Project/Scenes/Sandbox/Test_CombatArena.unity";
        private const string ItemsFolder = "Assets/_Project/Data/Items";
        private const string WeaponAssetPath = ItemsFolder + "/Weapon_IronSword_T1.asset";
        private const string MaterialsFolder = "Assets/_Project/Art/Materials";

        [MenuItem("Huyết Nguyệt Đại Ngàn/M3: Setup Combat Prototype Arena", false, 40)]
        public static void SetupM3CombatScene()
        {
            EnsureDirectories();

            Shader urpLit = Shader.Find("Universal Render Pipeline/Lit");
            if (urpLit == null) urpLit = Shader.Find("Standard");

            // 1. Materials
            Material groundMat = GetOrCreateMaterial(MaterialsFolder + "/M_CombatArena_Ground.mat", new Color(0.24f, 0.32f, 0.22f, 1f), urpLit);
            Material dummyMat = GetOrCreateMaterial(MaterialsFolder + "/M_Dummy_URP.mat", new Color(0.85f, 0.70f, 0.25f, 1f), urpLit);
            Material wolfMat = GetOrCreateMaterial(MaterialsFolder + "/M_Werewolf_URP.mat", new Color(0.22f, 0.24f, 0.28f, 1f), urpLit);
            Material healthFillMat = GetOrCreateMaterial(MaterialsFolder + "/M_HealthFill_URP.mat", new Color(0.92f, 0.15f, 0.15f, 1f), urpLit);
            Material healthBgMat = GetOrCreateMaterial(MaterialsFolder + "/M_HealthBg_URP.mat", new Color(0.12f, 0.12f, 0.12f, 1f), urpLit);
            Material wallMat = GetOrCreateMaterial(MaterialsFolder + "/M_ArenaWall_URP.mat", new Color(0.35f, 0.35f, 0.38f, 1f), urpLit);

            // 2. Weapon Data ScriptableObject
            WeaponDataSO swordData = GetOrCreateWeaponData();

            // 3. New Scene
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            // 4. Lighting & Sky
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.35f, 0.40f, 0.45f, 1f);

            var lightObj = new GameObject("Directional Light");
            var sunLight = lightObj.AddComponent<Light>();
            sunLight.type = LightType.Directional;
            sunLight.color = new Color(1f, 0.95f, 0.88f);
            sunLight.intensity = 1.3f;
            sunLight.shadows = LightShadows.Soft;
            lightObj.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

            // 5. Arena Ground (40m x 40m) & NavMesh Surface
            var groundObj = GameObject.CreatePrimitive(PrimitiveType.Plane);
            groundObj.name = "Arena_Floor";
            groundObj.transform.position = Vector3.zero;
            groundObj.transform.localScale = new Vector3(4f, 1f, 4f);
            groundObj.GetComponent<Renderer>().sharedMaterial = groundMat;

            // Boundary Walls
            CreateBoundaryWalls(groundObj, wallMat);

            // Bake NavMesh
            var navSurface = groundObj.AddComponent<NavMeshSurface>();
            navSurface.BuildNavMesh();

            // 6. Spawn Player with Combat
            GameObject player = CreatePlayerWithCombat(swordData, healthFillMat, healthBgMat);
            player.transform.position = new Vector3(0f, 0.1f, -4f);

            // 7. Spawn Combat Dummy (Bia tập chém)
            CreateCombatDummy(new Vector3(3.5f, 0f, 2.5f), dummyMat, healthFillMat, healthBgMat);

            // 8. Spawn Werewolf Tier 1 Prototype (Sói Bậc 1)
            CreateWerewolfPrototype(new Vector3(-6f, 0f, 6f), wolfMat, healthFillMat, healthBgMat);

            // 9. Camera with Cinemachine
            var camObj = new GameObject("Main Camera");
            camObj.tag = "MainCamera";
            var cam = camObj.AddComponent<Camera>();
            cam.fieldOfView = 40f;
            cam.nearClipPlane = 0.3f;
            cam.farClipPlane = 200f;
            camObj.AddComponent<UniversalAdditionalCameraData>();
            camObj.AddComponent<CinemachineBrain>();

            var vcamObj = new GameObject("CM_Combat_Vcam");
            var vcam = vcamObj.AddComponent<CinemachineVirtualCamera>();
            vcam.Follow = player.transform;
            var transposer = vcam.AddCinemachineComponent<CinemachineTransposer>();
            transposer.m_BindingMode = CinemachineTransposer.BindingMode.WorldSpace;
            transposer.m_FollowOffset = new Vector3(-14f, 16f, -14f);
            transposer.m_XDamping = 0.5f;
            transposer.m_YDamping = 0.5f;
            transposer.m_ZDamping = 0.5f;
            vcam.transform.rotation = Quaternion.Euler(45f, 45f, 0f);

            // 10. Runtime Info & Controls Help
            var infoObj = new GameObject("Runtime_Info_Display");
            infoObj.AddComponent<Core.RuntimeInfoDisplay>();

            // 11. Save Scene
            EditorSceneManager.SaveScene(scene, ScenePath);
            AddSceneToBuildSettings(ScenePath);

            AssetDatabase.Refresh();
            Debug.Log($"[Huyết Nguyệt M3] Đã tạo hoàn tất Scene '{ScenePath}' với Player, Bia tập chém và Sói Bậc 1!");
        }

        private static GameObject CreatePlayerWithCombat(WeaponDataSO weapon, Material fillMat, Material bgMat)
        {
            // Load existing prototype prefab if available
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

            // Add Health Component
            var health = player.GetComponent<HealthComponent>();
            if (health == null) health = player.AddComponent<HealthComponent>();
            health.SetMaxHealth(100f, true);

            // Add Hit Feedback
            if (player.GetComponent<HitFeedbackFlash>() == null)
            {
                player.AddComponent<HitFeedbackFlash>();
            }

            // Add Combat Controller
            var combat = player.GetComponent<PlayerCombatController>();
            if (combat == null) combat = player.AddComponent<PlayerCombatController>();
            combat.EquipWeapon(weapon);

            // Add Floating Health Bar over player
            AttachWorldHealthBar(player, fillMat, bgMat, new Vector3(0f, 2.2f, 0f), Color.green);

            return player;
        }

        private static void CreateCombatDummy(Vector3 position, Material dummyMat, Material fillMat, Material bgMat)
        {
            var dummy = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            dummy.name = "Combat_Dummy";
            dummy.transform.position = position + Vector3.up * 1f;
            dummy.transform.localScale = new Vector3(0.9f, 1f, 0.9f);
            dummy.GetComponent<Renderer>().sharedMaterial = dummyMat;

            var health = dummy.AddComponent<HealthComponent>();
            health.SetMaxHealth(200f, true);

            dummy.AddComponent<HitFeedbackFlash>();
            AttachWorldHealthBar(dummy, fillMat, bgMat, new Vector3(0f, 1.4f, 0f), Color.yellow);
        }

        private static void CreateWerewolfPrototype(Vector3 position, Material wolfMat, Material fillMat, Material bgMat)
        {
            var wolf = new GameObject("Enemy_Werewolf_Tier1");
            wolf.transform.position = position;

            // Visual Body
            var body = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            body.name = "Wolf_Visual_Body";
            body.transform.SetParent(wolf.transform);
            body.transform.localPosition = new Vector3(0f, 0.85f, 0f);
            body.transform.localScale = new Vector3(0.9f, 0.85f, 1.2f);
            body.GetComponent<Renderer>().sharedMaterial = wolfMat;

            // Visual Snout (Mõm sói hướng mặt Z+)
            var snout = GameObject.CreatePrimitive(PrimitiveType.Cube);
            snout.name = "Wolf_Snout";
            snout.transform.SetParent(wolf.transform);
            snout.transform.localPosition = new Vector3(0f, 1.1f, 0.7f);
            snout.transform.localScale = new Vector3(0.4f, 0.35f, 0.5f);
            snout.GetComponent<Renderer>().sharedMaterial = wolfMat;

            // Collider on root
            var capCol = wolf.AddComponent<CapsuleCollider>();
            capCol.height = 1.8f;
            capCol.radius = 0.5f;
            capCol.center = new Vector3(0f, 0.9f, 0f);

            // NavMeshAgent
            var agent = wolf.AddComponent<UnityEngine.AI.NavMeshAgent>();
            agent.height = 1.8f;
            agent.radius = 0.5f;
            agent.speed = 2.5f;

            // Health & AI
            var health = wolf.AddComponent<HealthComponent>();
            health.SetMaxHealth(80f, true);

            wolf.AddComponent<HitFeedbackFlash>();
            wolf.AddComponent<EnemyBrain>();

            // Floating Health Bar
            AttachWorldHealthBar(wolf, fillMat, bgMat, new Vector3(0f, 2.1f, 0f), Color.red);
        }

        private static void AttachWorldHealthBar(GameObject target, Material fillMat, Material bgMat, Vector3 offset, Color barColor)
        {
            var barRoot = new GameObject("HealthBar_World");
            barRoot.transform.SetParent(target.transform);
            barRoot.transform.localPosition = offset;

            // Background quad (1.2m x 0.18m)
            var bgObj = GameObject.CreatePrimitive(PrimitiveType.Quad);
            bgObj.name = "HealthBar_BG";
            bgObj.transform.SetParent(barRoot.transform);
            bgObj.transform.localPosition = Vector3.zero;
            bgObj.transform.localScale = new Vector3(1.2f, 0.18f, 1f);
            Object.DestroyImmediate(bgObj.GetComponent<Collider>());
            bgObj.GetComponent<Renderer>().sharedMaterial = bgMat;

            // Fill quad (1.16m x 0.14m)
            var fillObj = GameObject.CreatePrimitive(PrimitiveType.Quad);
            fillObj.name = "HealthBar_Fill";
            fillObj.transform.SetParent(barRoot.transform);
            fillObj.transform.localPosition = new Vector3(0f, 0f, -0.01f); // slightly in front
            fillObj.transform.localScale = new Vector3(1.16f, 0.14f, 1f);
            Object.DestroyImmediate(fillObj.GetComponent<Collider>());

            Material customFill = new Material(fillMat) { color = barColor };
            fillObj.GetComponent<Renderer>().sharedMaterial = customFill;

            var barScript = barRoot.AddComponent<WorldSpaceHealthBar>();
            var fieldHealth = typeof(WorldSpaceHealthBar).GetField("targetHealth", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var fieldFill = typeof(WorldSpaceHealthBar).GetField("fillTransform", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var fieldOffset = typeof(WorldSpaceHealthBar).GetField("offset", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            fieldHealth?.SetValue(barScript, target.GetComponent<HealthComponent>());
            fieldFill?.SetValue(barScript, fillObj.transform);
            fieldOffset?.SetValue(barScript, offset);
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

        private static void CreateBoundaryWalls(GameObject parent, Material mat)
        {
            float size = 20f;
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

        private static void EnsureDirectories()
        {
            if (!AssetDatabase.IsValidFolder("Assets/_Project/Data/Items"))
            {
                if (!AssetDatabase.IsValidFolder("Assets/_Project/Data"))
                {
                    AssetDatabase.CreateFolder("Assets/_Project", "Data");
                }
                AssetDatabase.CreateFolder("Assets/_Project/Data", "Items");
            }
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

