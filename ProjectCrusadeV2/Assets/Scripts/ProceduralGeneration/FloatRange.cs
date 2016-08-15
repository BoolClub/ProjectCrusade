using System;

[Serializable]
public class FloatRange {

	public float m_Min;
	public float m_Max;
	private float float_value;


	public FloatRange(float min, float max)
	{
		m_Min = min;
		m_Max = max;
	}

	public float Value
	{
		get { return float_value; }
		set { float_value = value; }
	}

	public float Random
	{
		get {
			float_value = UnityEngine.Random.Range(m_Min, m_Max);
			return float_value;
		}
	}
}
