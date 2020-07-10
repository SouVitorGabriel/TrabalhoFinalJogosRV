using UnityEngine;
using Google.Android.PerformanceTuner;
using System.Collections;

public class _GamePerformanceManager
{
    AndroidPerformanceTuner<FidelityParams, Annotation> tuner = new AndroidPerformanceTuner<FidelityParams, Annotation>();
    
    public IEnumerator Initialize()
    {
        yield return new WaitForEndOfFrame();
        ErrorCode startErrorCode = tuner.Start();
        Debug.Log("Android Performance Tuner started with code: " + startErrorCode);

        tuner.onReceiveUploadLog += request =>
        {
            Debug.Log("Telemetry uploaded with request name: " + request.name);
        };
    }
}
