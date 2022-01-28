using UnityEngine;

public abstract class AbstractVariable<T> : ScriptableObject{
	[SerializeField] private T m_value;

	public T value{
		get{
			return m_value;
		}
	}

	public void SetValue(T value){
		m_value = value;
	}

	public void SetValue(AbstractVariable<T> value){
		m_value = value.value;
	}

	public static implicit operator T(AbstractVariable<T> reference){
		return reference.value;
	}
}
