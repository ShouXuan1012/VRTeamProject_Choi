//

#region Namespaces

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

#endregion
namespace NextPrefabNameScape
{

public class NextPrefab : MonoBehaviour {
	
	#region Variables
	
		// Elements
		public GameObject[] m_PrefabList;

		
		// Index of current element
		int m_CurrentElementIndex = -1;
	
		// Index of current particle
		int m_CurrentParticleIndex = -1;	
		

		
		// Current particle list
		GameObject[] m_CurrentElementList = null;
	
		// GameObject of current particle that is showing in the scene
		GameObject m_CurrentParticle = null;
		
		    public Text m_ParticleName;
	
	#endregion
	
	// ######################################################################
	// MonoBehaviour Functions
	// ######################################################################

	#region Component Segments
		
		// Use this for initialization
		void Start () {
	
	  
			// Check if there is any particle in prefab list
			if(m_PrefabList.Length>0)
			{
				// reset indices of element and particle
				m_CurrentElementIndex = 0;
				m_CurrentParticleIndex = 0;
			
				// Show particle
				ShowParticle();
			}
		}
		
		// Update is called once per frame
       void Update()
        {
            // Проверка наличия частицы в списке префабов
            if (m_CurrentElementIndex != -1 && m_CurrentParticleIndex != -1)
            {
                // Пользователь отпустил клавишу Space
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    // Перезагрузка префаба
                    ShowParticle();
                }
                // Пользователь отпустил клавишу Left Arrow
                else if (Input.GetKeyUp(KeyCode.Z))
                {
                    m_CurrentParticleIndex--;
                    ShowParticle();
                }
                // Пользователь отпустил клавишу Right Arrow
                else if (Input.GetKeyUp(KeyCode.X))
                {
                    m_CurrentParticleIndex++;
                    ShowParticle();
                }
            }
        }

	#endregion Component Segments
	
	// ######################################################################
	// Functions Functions
	// ######################################################################

	#region Functions
		
		// Remove old Particle and do Create new Particle GameObject
		void ShowParticle()
		{
	
			
			// Update current m_CurrentElementList and m_ElementName
			if(m_CurrentElementIndex==0)
			{
				m_CurrentElementList = m_PrefabList;
			}
	
			// Make m_CurrentParticleIndex be rounded
			if(m_CurrentParticleIndex>=m_CurrentElementList.Length)
			{
				m_CurrentParticleIndex = 0;
			}
			else if(m_CurrentParticleIndex<0)
			{
				m_CurrentParticleIndex = m_CurrentElementList.Length-1;
			}
	
			// update current m_ParticleName
			m_ParticleName.text = m_CurrentElementList[m_CurrentParticleIndex].name;
	
			// Remove Old particle
			if(m_CurrentParticle!=null)
			{
				Destroy (m_CurrentParticle);
			}
	
			// Create new particle
			m_CurrentParticle = (GameObject) Instantiate(m_CurrentElementList[m_CurrentParticleIndex]);
		}
		


		
	#endregion {Functions}
}
}