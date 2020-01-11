using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

/// <summary>
/// Shakes camera. There should only be one of these in the scene!
/// </summary>
public class CameraShake : MonoBehaviour {

    public CameraShake instance; //Let's this be called by other scripts
    public PostProcessVolume[] volume;
    private List<ChromaticAberration> chromaticAberrationLayer;

    private void Start()
    {
        instance = this;
        volume = GetComponentsInChildren<PostProcessVolume>();
        chromaticAberrationLayer = new List<ChromaticAberration>();
        for (int i = 0; i < volume.Length; i++)
        {
            ChromaticAberration temp;
            volume[i].profile.TryGetSettings(out temp);
            if (temp != null)
                chromaticAberrationLayer.Add(temp);
        }       
    }

    /// <summary>
    /// Shakes the object attached to this by amound for duration
    /// </summary>
    /// <param name="amount">Shake amount</param>
    /// <param name="duration">Duration for shake</param>
    public void ShakeCaller (float amount, float duration)
    {
		StartCoroutine (Shake(amount, duration));
	}

	IEnumerator Shake (float amount, float duration){

		Vector3 originalPos = transform.localPosition;
		int counter = 0;

		while (duration > 0.01f) {
			counter++;

			var x = Random.Range (-1f, 1f) * (amount/counter);
			var y = Random.Range (-1f, 1f) * (amount/counter);

            for (int i = 0; i < chromaticAberrationLayer.Count; i++)
            {
                chromaticAberrationLayer[i].intensity.value = Random.Range(.5f, 2f) * (amount * 5f / counter);
            }
            

			transform.localPosition = Vector3.Lerp (transform.localPosition, new Vector3 (originalPos.x + x, originalPos.y + y, originalPos.z), 0.5f);

			duration -= Time.deltaTime;
			
			yield return new WaitForSeconds (0.01f);
		}

		transform.localPosition = originalPos;
	}
}
