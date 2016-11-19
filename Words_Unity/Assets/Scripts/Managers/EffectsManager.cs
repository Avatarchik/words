using UnityEngine;

public class EffectsManager : MonoBehaviour
{
	private readonly Vector3 kOffscreenPos = new Vector3(0, 5000, 0);

	[Range(1, 64)]
	public int PoolSize = 8;

	public ParticleSystem WordValidityEffectPrefab;
	private ParticleSystem[] mWordValidityEffectsPool;

	void Awake()
	{
		mWordValidityEffectsPool = new ParticleSystem[PoolSize];

		for (int poolIndex = 0; poolIndex < PoolSize; ++poolIndex)
		{
			mWordValidityEffectsPool[poolIndex] = CreateEffectInstance(WordValidityEffectPrefab, poolIndex);
		}
	}

	private ParticleSystem CreateEffectInstance(ParticleSystem effectPrefab, int poolIndex)
	{
		ParticleSystem psInstance = Instantiate(effectPrefab, kOffscreenPos, Quaternion.identity, transform) as ParticleSystem;

#if UNITY_EDITOR
		psInstance.gameObject.name = string.Format("{0} #{1}", effectPrefab.name, poolIndex);
#endif // UNITY_EDITOR

		return psInstance;
	}

	public void PlayWordValidityEffectAt(EWordValidityResult validityResult, Vector3 fromPosition, Vector3 toPosition)
	{
		ParticleSystem ps;
		for (int effectIndex = 0; effectIndex < PoolSize; ++effectIndex)
		{
			ps = mWordValidityEffectsPool[effectIndex];
			if (!ps.isPlaying)
			{
				if (toPosition.y < fromPosition.y)
				{
					Vector3 temp = fromPosition;
					fromPosition = toPosition;
					toPosition = temp;
				}

				Vector3 midPoint = Vector3.Lerp(fromPosition, toPosition, 0.5f);
				Vector3 difference = toPosition - fromPosition;

				float radius = difference.magnitude * 0.5f;
				short particlesPerBurst = (short)(radius * 32);

				ps.transform.position = midPoint;

				ParticleSystem.Burst[] bursts = new ParticleSystem.Burst[]
				{
					new ParticleSystem.Burst(0, particlesPerBurst, particlesPerBurst),
				};
				ParticleSystem.EmissionModule em = ps.emission;
				em.SetBursts(bursts);

				ParticleSystem.ShapeModule sm = ps.shape;
				sm.radius = radius;

				Vector3 rotation = ps.transform.rotation.eulerAngles;
				rotation.z = Vector3.Angle(difference, Vector3.right);
				ps.transform.eulerAngles = rotation;

				ps.startColor = GetWordValidityEffectColour(validityResult);

				ps.Play();
				break;
			}
		}
	}

	private Color GetWordValidityEffectColour(EWordValidityResult validityResult)
	{
		switch (validityResult)
		{
			case EWordValidityResult.WasRemoved:
				return GlobalSettings.Instance.FoundColour;

			case EWordValidityResult.WrongInstance:
				return GlobalSettings.Instance.WrongInstanceColour;

			case EWordValidityResult.WasAlreadyFound:
				return GlobalSettings.Instance.AlreadyFoundColour;

			default:
				return GlobalSettings.Instance.NotFoundColour;
		}
	}
}