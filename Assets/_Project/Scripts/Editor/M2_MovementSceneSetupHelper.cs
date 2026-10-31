using System.IO;
using Cinemachine;
using HuyetNguyetDaiNgan.Player;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace HuyetNguyetDaiNgan.Editor
{
    public static class M2_MovementSceneSetupHelper
    {
        private const string ScenePath = "Assets/_Project/Scenes/Sandbox/Test_Movement.unity";
        private const string PrefabFolder = "Assets/_Project/Prefabs/Player";
        private const string PlayerPrefabPath = PrefabFolder + "/Player_Prototype.prefab";
        private const string MaterialsFolder = "Assets/_Project/Art/Materials";

        [MenuItem("Huyết Nguyệt Đại Ngàn/M2: Setup Character & Test_Movement Scene", false, 30)]
        public static void SetupM2MovementScene()
        {
            // 1. Ensure folders exist
            EnsureDirectories();

            // 2. Create or load materials
            Shader urpLit = Shader.Find("Universal Render Pipeline/Lit");
            if (urpLit == null) urpLit = Shader.Find("Standard");

            Material groundMat = GetOrCreateMaterial(MaterialsFolder + "/M_Ground_URP.mat", new Color(0.25f, 0.38f, 0.22f, 1f), urpLit);
            Material slopeMat = GetOrCreateMaterial(MaterialsFolder + "/M_Slope_URP.mat", new Color(0.42f, 0.38f, 0.30f, 1f), urpLit);
            Material obstacleMat = GetOrCreateMaterial(MaterialsFolder + "/M_Obstacle_URP.mat", new Color(0.45f, 0.45f, 0.50f, 1f), urpLit);
            Material playerBodyMat = GetOrCreateMaterial(MaterialsFolder + "/M_PlayerBody_URP.mat", new Color(0.18f, 0.45f, 0.75f, 1f), urpLit);
            Material playerVisorMat = GetOrCreateMaterial(MaterialsFolder + "/M_PlayerVisor_URP.mat", new Color(0.95f, 0.75f, 0.15f, 1f), urpLit);

            // 3. Create or Update Player Prototype Prefab
            GameObject playerPrefab = CreateOrUpdatePlayerPrefab(playerBodyMat, playerVisorMat);

            // 4. Create new scene or open Test_Movement.unity
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            // 5. Environment Lighting & Sky
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.35f, 0.40f, 0.45f, 1f);

            // 6. Directional Light
            var lightObj = new GameObject("Directional Light");
            var lightComp = lightObj.AddComponent<Light>();
            lightComp.type = LightType.Directional;
            lightComp.color = new Color(1f, 0.96f, 0.88f);
            lightComp.intensity = 1.25f;
            lightComp.shadows = LightShadows.Soft;
            lightObj.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

            // 7. Ground Area (50m x 50m)
            var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Ground_Arena";
            ground.transform.position = Vector3.zero;
            ground.transform.localScale = new Vector3(5f, 1f, 5f);
            ground.GetComponent<Renderer>().sharedMaterial = groundMat;

            // 8. Test Obstacles & Slopes
            BuildTestCourse(slopeMat, obstacleMat);

            // 9. Instantiate Player
            GameObject playerInstance = (GameObject)PrefabUtility.InstantiatePrefab(playerPrefab);
            playerInstance.name = "Player";
            playerInstance.transform.position = new Vector3(0f, 0.1f, 0f);

            // 10. Main Camera with CinemachineBrain
            var camObj = new GameObject("Main Camera");
            camObj.tag = "MainCamera";
            var cam = camObj.AddComponent<Camera>();
            cam.fieldOfView = 40f;
            cam.nearClipPlane = 0.3f;
            cam.farClipPlane = 200f;
            camObj.AddComponent<UniversalAdditionalCameraData>();
            camObj.AddComponent<CinemachineBrain>();

            // 11. Cinemachine Virtual Camera (Isometric 3/4)
            var vcamObj = new GameObject("CM_Isometric_Vcam");
            var vcam = vcamObj.AddComponent<CinemachineVirtualCamera>();
            vcam.Follow = playerInstance.transform;

            // Configure Transposer in World Space for fixed 3/4 isometric follow
            var transposer = vcam.AddCinemachineComponent<CinemachineTransposer>();
            transposer.m_BindingMode = CinemachineTransposer.BindingMode.WorldSpace;
            transposer.m_FollowOffset = new Vector3(-14f, 16f, -14f);
            transposer.m_XDamping = 0.6f;
            transposer.m_YDamping = 0.6f;
            transposer.m_ZDamping = 0.6f;

            vcam.transform.rotation = Quaternion.Euler(45f, 45f, 0f);

            // 12. Add Runtime Info Display (FPS & Controls)
            var infoObj = new GameObject("Runtime_Info_Display");
            infoObj.AddComponent<Core.RuntimeInfoDisplay>();

            // 13. Save Scene
            EditorSceneManager.SaveScene(scene, ScenePath);

            // Add to Build Settings
            AddSceneToBuildSettings(ScenePath);

            AssetDatabase.Refresh();
            Debug.Log($"[Huyết Nguyệt M2] Đã khởi tạo hoàn tất Scene '{ScenePath}' và Prefab '{PlayerPrefabPath}'!");
        }

        private static GameObject CreateOrUpdatePlayerPrefab(Material bodyMat, Material visorMat)
        {
            GameObject root = new GameObject("Player_Prototype");

            // CharacterController
            var cc = root.AddComponent<CharacterController>();
            cc.height = 1.8f;
            cc.radius = 0.4f;
            cc.center = new Vector3(0f, 0.9f, 0f);
            cc.slopeLimit = 45f;
            cc.stepOffset = 0.35f;

            // Input & Movement
            root.AddComponent<PlayerInputHandler>();
            var movement = root.AddComponent<PlayerMovementController>();

            // Decoupled Visual Root
            GameObject visual = new GameObject("PlayerVisual");
            visual.transform.SetParent(root.transform);
            visual.transform.localPosition = Vector3.zero;
            visual.transform.localRotation = Quaternion.identity;

            // Visual Body (Capsule)
            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            body.name = "Visual_Body";
            body.transform.SetParent(visual.transform);
            body.transform.localPosition = new Vector3(0f, 0.9f, 0f);
            body.transform.localScale = new Vector3(0.8f, 0.9f, 0.8f);
            Object.DestroyImmediate(body.GetComponent<Collider>()); // Xóa collider con tránh xung đột CharacterController
            body.GetComponent<Renderer>().sharedMaterial = bodyMat;

            // Visual Visor / Direction Pointer (Khối mỏ nhọn hướng mặt Z+)
            GameObject visor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            visor.name = "Visual_Visor";
            visor.transform.SetParent(visual.transform);
            visor.transform.localPosition = new Vector3(0f, 1.35f, 0.45f);
            visor.transform.localScale = new Vector3(0.35f, 0.18f, 0.4f);
            Object.DestroyImmediate(visor.GetComponent<Collider>());
            visor.GetComponent<Renderer>().sharedMaterial = visorMat;

            // Bridge
            visual.AddComponent<PlayerVisualAnimationBridge>();

            // Save Prefab
            GameObject savedPrefab = PrefabUtility.SaveAsPrefabAsset(root, PlayerPrefabPath);
            Object.DestroyImmediate(root);
            return savedPrefab;
        }

        private static void BuildTestCourse(Material slopeMat, Material obstacleMat)
        {
            GameObject courseRoot = new GameObject("Test_Obstacle_Course");

            // 1. Dốc 15 độ (Dốc thoai thoải)
            CreateSlope(courseRoot, "Slope_15deg", new Vector3(-12f, 1f, 8f), 15f, slopeMat);

            // 2. Dốc 30 độ (Dốc trung bình)
            CreateSlope(courseRoot, "Slope_30deg", new Vector3(-6f, 1.8f, 8f), 30f, slopeMat);

            // 3. Dốc 45 độ (Dốc giới hạn leo)
            CreateSlope(courseRoot, "Slope_45deg", new Vector3(0f, 2.5f, 8f), 45f, slopeMat);

            // 4. Dốc 60 độ (Dốc cấm leo - quá dốc, nhân vật sẽ trượt)
            CreateSlope(courseRoot, "Slope_60deg_Steep", new Vector3(6f, 3f, 8f), 60f, obstacleMat);

            // 5. Bậc thang thử nghiệm Step Offset (Cao 0.2m leo được vs 0.6m bị cản)
            CreateStep(courseRoot, "Step_0.2m_Passable", new Vector3(12f, 0.1f, 5f), new Vector3(3f, 0.2f, 2f), slopeMat);
            CreateStep(courseRoot, "Step_0.6m_Wall", new Vector3(12f, 0.3f, 8f), new Vector3(3f, 0.6f, 2f), obstacleMat);

            // 6. Cột chướng ngại vật (Trụ đá)
            CreatePillar(courseRoot, "Stone_Pillar_A", new Vector3(-8f, 1.5f, -6f), obstacleMat);
            CreatePillar(courseRoot, "Stone_Pillar_B", new Vector3(8f, 1.5f, -6f), obstacleMat);

            // 7. Rào chắn biên giới (Tường bao)
            CreateBorderWalls(courseRoot, obstacleMat);
        }

        private static void CreateSlope(GameObject parent, string name, Vector3 pos, float angleDeg, Material mat)
        {
            var slope = GameObject.CreatePrimitive(PrimitiveType.Cube);
            slope.name = name;
            slope.transform.SetParent(parent.transform);
            slope.transform.position = pos;
            slope.transform.rotation = Quaternion.Euler(-angleDeg, 0f, 0f);
            slope.transform.localScale = new Vector3(3f, 0.4f, 6f);
            slope.GetComponent<Renderer>().sharedMaterial = mat;
        }

        private static void CreateStep(GameObject parent, string name, Vector3 pos, Vector3 size, Material mat)
        {
            var step = GameObject.CreatePrimitive(PrimitiveType.Cube);
            step.name = name;
            step.transform.SetParent(parent.transform);
            step.transform.position = pos;
            step.transform.localScale = size;
            step.GetComponent<Renderer>().sharedMaterial = mat;
        }

        private static void CreatePillar(GameObject parent, string name, Vector3 pos, Material mat)
        {
            var pillar = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            pillar.name = name;
            pillar.transform.SetParent(parent.transform);
            pillar.transform.position = pos;
            pillar.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            pillar.GetComponent<Renderer>().sharedMaterial = mat;
        }

        private static void CreateBorderWalls(GameObject parent, Material mat)
        {
            float size = 25f;
            float thickness = 1f;
            float height = 2f;

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
            if (!AssetDatabase.IsValidFolder("Assets/_Project/Prefabs/Player"))
            {
                if (!AssetDatabase.IsValidFolder("Assets/_Project/Prefabs"))
                {
                    AssetDatabase.CreateFolder("Assets/_Project", "Prefabs");
                }
                AssetDatabase.CreateFolder("Assets/_Project/Prefabs", "Player");
            }
            if (!AssetDatabase.IsValidFolder("Assets/_Project/Scenes/Sandbox"))
            {
                AssetDatabase.CreateFolder("Assets/_Project/Scenes", "Sandbox");
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

