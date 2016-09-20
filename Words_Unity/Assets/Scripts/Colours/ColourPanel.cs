using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[ScriptOrder(-99)]
public class ColourPanel : MonoBehaviour
{
	public GameObject ColourPanelEntryPrefab;

	[HideInInspector]
	public ColourScheme Scheme;

	private List<Image> mPanelEntries;

	private int mMaxCharUsage;

	public void Initialise(int maxCharUsage)
	{
		CleanUp();

		mMaxCharUsage = maxCharUsage;

		float entrySize = 24;
		float entryGap = 4;
		float totalSize = (entrySize * (mMaxCharUsage - 1)) + (entryGap * (mMaxCharUsage - 1));

		Vector3 pos = new Vector3(entrySize * 1.5f, totalSize * -0.5f, 0);

		mPanelEntries = new List<Image>(mMaxCharUsage);
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

	private void CleanUp()
	{
		if (mPanelEntries != null)
		{
			for (int entryIndex = (mPanelEntries.Count - 1); entryIndex >= 0; --entryIndex)
			{
				if (mPanelEntries[entryIndex] != null)
				{
					Destroy(mPanelEntries[entryIndex]);
				}
			}
			mPanelEntries = null;
		}

		mMaxCharUsage = 0;
	}

	void OnEnable()
	{
		ColourSchemesManager.OnSchemeSwitched += OnSchemeSwitched;
	}

	void OnDisable()
	{
		ColourSchemesManager.OnSchemeSwitched -= OnSchemeSwitched;
	}

	private void OnSchemeSwitched(ColourScheme newScheme)
	{
		Scheme = newScheme;
		UpdateColour();
	}

	private void UpdateColour()
	{
		if (mPanelEntries != null)
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
}