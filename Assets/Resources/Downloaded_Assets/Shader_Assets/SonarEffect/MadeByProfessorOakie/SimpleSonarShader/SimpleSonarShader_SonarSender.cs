using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSonarShader_SonarSender : MonoBehaviour
{
    #region Singleton - has Awake function
    private static SimpleSonarShader_SonarSender _instance;

    public static SimpleSonarShader_SonarSender Instance { 
        get {
            if (!_instance) {
                GameObject sender = new GameObject("Sonar Sender Singleton");
                _instance = sender.AddComponent<SimpleSonarShader_SonarSender>();
                _instance.Start();
            }
            return _instance; 
        } }

    private void Awake()
    {
        if (_instance != null && _instance != this) {
            Destroy(this);
        }
        else {
            _instance = this;
        }
    }
    #endregion

    // Throwaway values to set position to at the start.
    private static readonly Vector4 GarbagePosition = new Vector4(-5000, -5000, -5000, -5000);

    // The number of rings that can be rendered at once.
    // Must be the same value as the RingArraySize in the shader file SimpleSonarCore.hlsl.
    // You should adjust this to the max number of rings you will need at once. If you have performance issues then bring that number down. 
    private static int QueueSize = 64;

    // Queue of start positions of sonar rings.
    // The xyz values hold the xyz of position.
    // The w value holds the time that position was started.
    private Queue<Vector4> positionsQueue = new Queue<Vector4>(QueueSize);

    // Queue of intensity values for each ring.
    // These are kept in the same order as the positionsQueue.
    private Queue<float> intensityQueue = new Queue<float>(QueueSize);
    
    // Queue of intensity values for each ring.
    // These are kept in the same order as the positionsQueue.
    private Queue<float> ringPassIndexQueue = new Queue<float>(QueueSize);

    protected virtual void Start()
    {
        if(positionsQueue.Count == 0)
        {
            // Fill queues with starting values that are garbage values
            for (int i = 0; i < QueueSize; i++)
            {
                positionsQueue.Enqueue(GarbagePosition);
                intensityQueue.Enqueue(-5000f);
                ringPassIndexQueue.Enqueue(0);
            }
        }
    }

    /// <summary>
    /// Starts a sonar ring from this position with the given intensity.
    /// </summary>
    public void StartSonarRing(Vector4 position, float intensity, int ringPassIndex = 0)
    {
        // Put values into the queue
        position.w = Time.time;
        positionsQueue.Dequeue();
        positionsQueue.Enqueue(position);

        intensityQueue.Dequeue();
        intensityQueue.Enqueue(intensity);

        ringPassIndexQueue.Dequeue();
        ringPassIndexQueue.Enqueue(ringPassIndex);

        Shader.SetGlobalVectorArray("_hitPts", positionsQueue.ToArray());
        Shader.SetGlobalFloatArray("_Intensity", intensityQueue.ToArray());
        Shader.SetGlobalFloatArray("_RingPassIndex", ringPassIndexQueue.ToArray());
    }
}
