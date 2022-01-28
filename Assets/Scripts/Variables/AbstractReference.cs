using UnityEngine;


[System.Serializable]
public abstract class AbstractReference<T> {

	[SerializeField]
	protected bool m_useConstant = false;

	[SerializeField]
	protected T m_constantValue;

	[SerializeField]
	protected AbstractVariable<T> m_variable;

	public AbstractReference(){}
	public AbstractReference(T value){
		m_useConstant = true;
		m_constantValue = value;
	}

	public T value{
		get{
			return m_useConstant ? m_constantValue : m_variable.value;
		}
	}

	public void SetValue(T value){
		if(m_useConstant){
			m_constantValue = value;
		}else{
			m_variable.SetValue(value);
		}
	}

	public static implicit operator T(AbstractReference<T> reference){
		return reference.value;
	}
}