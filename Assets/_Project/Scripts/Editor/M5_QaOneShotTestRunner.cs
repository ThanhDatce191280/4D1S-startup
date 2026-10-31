using System.IO;
using UnityEditor;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;

namespace HuyetNguyetDaiNgan.Editor
{
    internal static class M5QaOneShotTestRunner
    {
        private static string RequestPath => Path.GetFullPath(Path.Combine(Application.dataPath, "../Temp/M5QaEditMode.request"));
        private static string ResultPath => Path.GetFullPath(Path.Combine(Application.dataPath, "../Temp/M5QaEditModeResults.xml"));
        private static TestRunnerApi _api;
        private static ResultCallback _callback;

        [InitializeOnLoadMethod]
        private static void QueueRequestedRun()
        {
            if (!File.Exists(RequestPath)) return;
            EditorApplication.delayCall += Run;
        }

        private static void Run()
        {
            if (!File.Exists(RequestPath)) return;
            File.Delete(RequestPath);
            if (File.Exists(ResultPath)) File.Delete(ResultPath);

            _api = ScriptableObject.CreateInstance<TestRunnerApi>();
            _callback = new ResultCallback();
            _api.RegisterCallbacks(_callback);
            _api.Execute(new ExecutionSettings(new Filter
            {
                testMode = TestMode.EditMode,
                assemblyNames = new[] { "HuyetNguyetDaiNgan.Tests.EditMode" }
            })
            {
                runSynchronously = true
            });
        }

        private sealed class ResultCallback : ICallbacks
        {
            public void RunStarted(ITestAdaptor testsToRun) { }
            public void TestStarted(ITestAdaptor test) { }
            public void TestFinished(ITestResultAdaptor result) { }

            public void RunFinished(ITestResultAdaptor result)
            {
                TestRunnerApi.SaveResultToFile(result, ResultPath);
                _api.UnregisterCallbacks(this);
                Object.DestroyImmediate(_api);
                _api = null;
                _callback = null;
                Debug.Log($"[M5 QA] EditMode run finished: {result.ResultState}; pass={result.PassCount}, fail={result.FailCount}, skip={result.SkipCount}. Result: {ResultPath}");
            }
        }
    }
}
