using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

namespace HuyetNguyetDaiNgan.Editor
{
    public static class SceneSetupAndBuildHelper
    {
        private const string ScenePath = "Assets/_Project/Scenes/Sandbox/Sandbox_Foundation.unity";
        private const string MaterialsFolder = "Assets/_Project/Art/Materials";
        private const string GroundMaterialPath = MaterialsFolder + "/M_Ground_URP.mat";
        private const string MarkerMaterialPath = MaterialsFolder + "/M_Marker_URP.mat";

        [MenuItem("Huyết Nguyệt Đại Ngàn/1. Setup Sandbox Foundation Scene", false, 10)]
        public static void SetupSandboxFoundationScene()
        {
            // 1. Ensure folder exists
            if (!AssetDatabase.IsValidFolder("Assets/_Project/Art/Materials"))
            {
                if (!AssetDatabase.IsValidFolder("Assets/_Project/Art"))
                {
                    AssetDatabase.CreateFolder("Assets/_Project", "Art");
                }
                AssetDatabase.CreateFolder("Assets/_Project/Art", "Materials");
            }

            // 2. Open or create the scene
            var scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

            // 3. Create or load materials
            Shader urpLitShader = Shader.Find("Universal Render Pipeline/Lit");
            if (urpLitShader == null)
            {
                urpLitShader = Shader.Find("Standard");
            }

            Material groundMat = AssetDatabase.LoadAssetAtPath<Material>(GroundMaterialPath);
            if (groundMat == null)
            {
                groundMat = new Material(urpLitShader);
                groundMat.name = "M_Ground_URP";
                groundMat.color = new Color(0.22f, 0.35f, 0.20f, 1f); // Rừng rậm xanh rêu đậm
                AssetDatabase.CreateAsset(groundMat, GroundMaterialPath);
            }

            Material markerMat = AssetDatabase.LoadAssetAtPath<Material>(MarkerMaterialPath);
            if (markerMat == null)
            {
                markerMat = new Material(urpLitShader);
                markerMat.name = "M_Marker_URP";
                markerMat.color = new Color(0.85f, 0.45f, 0.18f, 1f); // Màu đất nung / gỗ ấm
                AssetDatabase.CreateAsset(markerMat, MarkerMaterialPath);
            }

            AssetDatabase.SaveAssets();

            // 4. Setup Ground Plane
            GameObject groundObj = GameObject.Find("Ground_Plane");
            if (groundObj == null)
            {
                groundObj = GameObject.CreatePrimitive(PrimitiveType.Plane);
                groundObj.name = "Ground_Plane";
            }
            groundObj.transform.position = Vector3.zero;
            groundObj.transform.rotation = Quaternion.identity;
            groundObj.transform.localScale = new Vector3(3f, 1f, 3f); // 30m x 30m
            var groundRenderer = groundObj.GetComponent<Renderer>();
            if (groundRenderer != null)
            {
                groundRenderer.sharedMaterial = groundMat;
            }

            // 5. Setup Marker Cube (Vật thể định vị trung tâm)
            GameObject markerObj = GameObject.Find("Center_Marker");
            if (markerObj == null)
            {
                markerObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                markerObj.name = "Center_Marker";
            }
            markerObj.transform.position = new Vector3(0f, 0.5f, 0f);
            markerObj.transform.rotation = Quaternion.identity;
            markerObj.transform.localScale = Vector3.one;
            var markerRenderer = markerObj.GetComponent<Renderer>();
            if (markerRenderer != null)
            {
                markerRenderer.sharedMaterial = markerMat;
            }

            // 6. Setup Isometric 3/4 Camera
            Camera mainCam = Camera.main;
            if (mainCam == null)
            {
                var camObj = new GameObject("Main Camera");
                mainCam = camObj.AddComponent<Camera>();
                camObj.tag = "MainCamera";
            }
            // Góc Isometric 3/4 kiểu Albion Online: Nhìn từ góc Tây Nam chếch 45 độ
            mainCam.transform.position = new Vector3(-14f, 16f, -14f);
            mainCam.transform.rotation = Quaternion.Euler(45f, 45f, 0f);
            mainCam.fieldOfView = 40f;
            mainCam.nearClipPlane = 0.3f;
            mainCam.farClipPlane = 100f;

            // 7. Setup Directional Light
            Light sunLight = null;
            var lights = Object.FindObjectsByType<Light>(FindObjectsSortMode.None);
            foreach (var l in lights)
            {
                if (l.type == LightType.Directional)
                {
                    sunLight = l;
                    break;
                }
            }
            if (sunLight == null)
            {
                var lightObj = new GameObject("Directional Light");
                sunLight = lightObj.AddComponent<Light>();
                sunLight.type = LightType.Directional;
            }
            sunLight.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
            sunLight.color = new Color(1f, 0.96f, 0.88f);
            sunLight.intensity = 1.2f;
            sunLight.shadows = LightShadows.Soft;

            // 8. Setup Runtime Info Display (FPS & Version)
            GameObject infoObj = GameObject.Find("Runtime_Info_Display");
            if (infoObj == null)
            {
                infoObj = new GameObject("Runtime_Info_Display");
                infoObj.AddComponent<Core.RuntimeInfoDisplay>();
            }

            // 9. Add Scene to EditorBuildSettings if not present
            var buildScenes = EditorBuildSettings.scenes;
            bool sceneExistsInBuild = false;
            foreach (var s in buildScenes)
            {
                if (s.path == ScenePath)
                {
                    sceneExistsInBuild = true;
                    break;
                }
            }

            if (!sceneExistsInBuild)
            {
                var newScenes = new EditorBuildSettingsScene[buildScenes.Length + 1];
                buildScenes.CopyTo(newScenes, 0);
                newScenes[newScenes.Length - 1] = new EditorBuildSettingsScene(ScenePath, true);
                EditorBuildSettings.scenes = newScenes;
                Debug.Log($"[Huyết Nguyệt] Đã thêm scene '{ScenePath}' vào Build Settings!");
            }

            // 9. Save scene
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            AssetDatabase.Refresh();

            Debug.Log("[Huyết Nguyệt] Đã thiết lập hoàn tất Scene 'Sandbox_Foundation' với mặt sàn Plane 30m, góc camera Isometric 3/4 và ánh sáng URP!");
        }

        [MenuItem("Huyết Nguyệt Đại Ngàn/2. Build Windows Standalone (.exe)", false, 20)]
        public static void BuildWindowsStandalone()
        {
            // Đảm bảo scene được setup trước khi build
            SetupSandboxFoundationScene();

            string buildDir = "Builds/Windows";
            if (!Directory.Exists(buildDir))
            {
                Directory.CreateDirectory(buildDir);
            }

            string exePath = Path.Combine(buildDir, "HuyetNguyetDaiNgan_TestBuild.exe");

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = new[] { ScenePath },
                locationPathName = exePath,
                target = BuildTarget.StandaloneWindows64,
                options = BuildOptions.None
            };

            Debug.Log($"[Huyết Nguyệt] Bắt đầu quá trình Build Standalone ra: {exePath}...");
            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"[Huyết Nguyệt] BUILD THÀNH CÔNG RỰC RỠ! Dung lượng: {summary.totalSize / (1024 * 1024)} MB trong {summary.totalTime.TotalSeconds:F1}s.");
                EditorUtility.RevealInFinder(exePath);
            }
            else if (summary.result == BuildResult.Failed)
            {
                Debug.LogError($"[Huyết Nguyệt] BUILD THẤT BẠI! Số lỗi: {summary.totalErrors}");
            }
        }
    }
}
