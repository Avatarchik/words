using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ColourPanel : MonoBehaviour
{
	public GameObject ColourPanelEntryPrefab;

	public Color FromColour;
	public Color ToColour;

	public Generator GeneratorRef;

	private List<GameObject> mPanelEntries = new List<GameObject>();

	void Start()
	{
		int maxCharUsage = GeneratorRef.MaxCharUsage;

		float entrySize = 24;
		float entryGap = 4;
		float totalSize = (entrySize * maxCharUsage) + (entryGap * (maxCharUsage - 1));

		Vector3 pos = new Vector3(entrySize, totalSize * -0.5f, 0);

		for (int i = 0; i < maxCharUsage; ++i)
		{
			GameObject entry = Instantiate(ColourPanelEntryPrefab, Vector3.zero, Quaternion.identity, transform) as GameObject;
			entry.transform.SetParent(transform);

			RectTransform rectTrans = entry.GetComponent<RectTransform>();
			rectTrans.localPosition = pos;
			pos.y += entrySize + entryGap;

			Image image = entry.GetComponent<Image>();
			image.color = ColorHelper.Blend(FromColour, ToColour, (1f / maxCharUsage) * i);

			mPanelEntries.Add(entry);
		}
	}
}