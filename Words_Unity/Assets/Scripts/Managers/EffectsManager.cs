using UnityEngine;

public class EffectsManager : MonoBehaviour
{
	private int kPoolSize = 32;
	private readonly Vector3 kOffscreenPos = new Vector3(0, 5000, 0);

	public ParticleSystem FoundEffectPrefab;
	public ParticleSystem NotFoundEffectPrefab;
	public ParticleSystem AlreadyFoundEffectPrefab;
	public ParticleSystem WrongInstanceEffectPrefab;

	private ParticleSystem[] mFoundEffectsPool;
	private ParticleSystem[] mNotFoundEffectsPool;
	private ParticleSystem[] mAlreadyFoundEffectsPool;
	private ParticleSystem[] mWrongInstanceEffectsPool;

	void Awake()
	{
		mFoundEffectsPool = new ParticleSystem[kPoolSize];
		mNotFoundEffectsPool = new ParticleSystem[kPoolSize];
		mAlreadyFoundEffectsPool = new ParticleSystem[kPoolSize];
		mWrongInstanceEffectsPool = new ParticleSystem[kPoolSize];

		for (int poolIndex = 0; poolIndex < kPoolSize; ++poolIndex)
		{
			mFoundEffectsPool[poolIndex] = Instantiate(FoundEffectPrefab, kOffscreenPos, Quaternion.identity, transform) as ParticleSystem;
			mNotFoundEffectsPool[poolIndex] = Instantiate(NotFoundEffectPrefab, kOffscreenPos, Quaternion.identity, transform) as ParticleSystem;
			mAlreadyFoundEffectsPool[poolIndex] = Instantiate(AlreadyFoundEffectPrefab, kOffscreenPos, Quaternion.identity, transform) as ParticleSystem;
			mWrongInstanceEffectsPool[poolIndex] = Instantiate(WrongInstanceEffectPrefab, kOffscreenPos, Quaternion.identity, transform) as ParticleSystem;
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

	public void PlayWrongInstanceEffectAt(Vector3 worldPosition)
	{
		PlayEffectAt(worldPosition, mWrongInstanceEffectsPool);
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