using UnityEngine;

public class EffectsManager : MonoBehaviour
{
	private int kPoolSize = 32;
	private readonly Vector3 kOffscreenPos = new Vector3(0, 5000, 0);

	public ParticleSystem WordFoundEffectPrefab;
	public ParticleSystem WordNotFoundEffectPrefab;
	public ParticleSystem WordAlreadyFoundEffectPrefab;

	private ParticleSystem[] mFoundEffectsPool;
	private ParticleSystem[] mNotFoundEffectsPool;
	private ParticleSystem[] mAlreadyFoundEffectsPool;

	void Awake()
	{
		mFoundEffectsPool = new ParticleSystem[kPoolSize];
		mNotFoundEffectsPool = new ParticleSystem[kPoolSize];
		mAlreadyFoundEffectsPool = new ParticleSystem[kPoolSize];

		for (int poolIndex = 0; poolIndex < kPoolSize; ++poolIndex)
		{
			mFoundEffectsPool[poolIndex] = Instantiate(WordFoundEffectPrefab, kOffscreenPos, Quaternion.identity, transform) as ParticleSystem;
			mNotFoundEffectsPool[poolIndex] = Instantiate(WordNotFoundEffectPrefab, kOffscreenPos, Quaternion.identity, transform) as ParticleSystem;
			mAlreadyFoundEffectsPool[poolIndex] = Instantiate(WordAlreadyFoundEffectPrefab, kOffscreenPos, Quaternion.identity, transform) as ParticleSystem;
		}
	}

	public void PlayFoundEffectAt(Vector3 worldPosition)
	{
		PlayEffectAt(worldPosition, mFoundEffectsPool);
	}

	public void PlayNotFoundEffectAt(Vector3 worldPosition)
	{
		PlayEffectAt(worldPosition, mNotFoundEffectsPool);
	}

	public void PlayAlreadyFoundEffectAt(Vector3 worldPosition)
	{
		PlayEffectAt(worldPosition, mAlreadyFoundEffectsPool);
	}

	private void PlayEffectAt(Vector3 worldPosition, ParticleSystem[] effectArray)
	{
		ParticleSystem ps;
		for (int effectIndex = 0; effectIndex < kPoolSize; ++effectIndex)
		{
			ps = effectArray[effectIndex];
			if (!ps.isPlaying)
			{
				ps.transform.position = worldPosition;
				ps.Play();
				break;
			}
		}
	}
}