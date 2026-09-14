using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// // public class SliderController : MonoBehaviour

// {

//     public DualQuaternionPoseDemo demo;

//     public Slider slider;

//     void Start()

//     {

//         slider.onValueChanged.AddListener(UpdateU);

//         // Set initial value

//         slider.value = 0.5f;

//         // Update the demo immediately

//         UpdateU(slider.value);

//     }

//     void UpdateU(float value)

//     {

//         demo.u = value;

//     }

// }

public class SliderController : MonoBehaviour

{

    public DualQuaternionPoseDemo dualQuaternionDemo;

    public StandardInterpolationDemo standardDemo;

    public Slider slider;

    void Start()

    {

        slider.onValueChanged.AddListener(UpdateU);

        slider.value = 0.5f;

        UpdateU(slider.value);

    }

    void UpdateU(float value)

    {

        dualQuaternionDemo.u = value;

        standardDemo.u = value;

    }

}