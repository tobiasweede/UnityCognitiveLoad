using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;

public class LslOutlet : MonoBehaviour
{
    string StreamName = "eeg";
    string StreamType = "Markers";
    private StreamOutlet outlet;
    private string[] sample = { "" };

    void Start()
    {
        var hash = new Hash128();
        hash.Append(StreamName);
        hash.Append(StreamType);
        hash.Append(gameObject.GetInstanceID());
        StreamInfo streamInfo = new StreamInfo(StreamName, StreamType, 1, LSL.LSL.IRREGULAR_RATE,
            channel_format_t.cf_string, hash.ToString());
        outlet = new StreamOutlet(streamInfo);
    }


    public void Send(string msg)
    {
        if (outlet != null)
        {
            sample[0] = msg;
            outlet.push_sample(sample);
        }
    }
}