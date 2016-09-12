using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ColourPanel : MonoBehaviour
{
	public GameObject ColourPanelEntryPrefab;

	public ColourScheme Scheme;

	private List<Image> mPanelEntries = new List<Image>();

	private int mMaxCharUsage;

	void Start()
	{
		mMaxCharUsage = Generator.Instance.MaxCharacterUsage;

		float entrySize = 24;
		float entryGap = 4;
		float totalSize = (entrySize * (mMaxCharUsage - 1)) + (entryGap * (mMaxCharUsage - 1));

		Vector3 pos = new Vector3(entrySize * 1.5f, totalSize * -0.5f, 0);

		for (int i = 0; i < mMaxCharUsage; ++i)
		{
			GameObject entry = Instantiate(ColourPanelEntryPrefab, Vector3.zero, Quaternion.identity, transform) as GameObject;
			entry.transform.SetParent(transform);
			entry.name = "Colour #" + (i + 1);

			RectTransform rectTrans = entry.GetComponent<RectTransform>();
			rectTrans.localPosition = pos;
			pos.y += entrySize + entryGap;

			Image image = entry.GetComponent<Image>();

			mPanelEntries.Add(image);
		}

		UpdateColour();
	}

	void OnEnable()
	{
		ColourSwitcher.OnColourSwitched += OnColourSwitched;
	}

	void OnDisable()
	{
		ColourSwitcher.OnColourSwitched -= OnColourSwitched;
	}

	private void OnColourSwitched(ColourScheme newScheme)
	{
		Scheme = newScheme;
		UpdateColour();
	}

	private void UpdateColour()
	{
		for (int entryIndex = 0; entryIndex < mPanelEntries.Count; ++entryIndex)
		{
			Image image = mPanelEntries[entryIndex];

			float t = (1f / (mMaxCharUsage - 1)) * entryIndex;
			t = MathfHelper.Clamp01(t);

			image.color = ColorHelper.Blend(Scheme.High, Scheme.Low, t);
		}
	}
}