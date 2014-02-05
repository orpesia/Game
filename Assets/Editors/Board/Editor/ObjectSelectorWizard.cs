//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.18408
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using UnityEditor;

using System.Collections.Generic;

using Unnamed;

namespace BoardEditor
{
	public class ObjectSelectorWizard : ScriptableWizard
	{		
		private System.Action<Object> m_delegate;
		private System.Func<Object, Texture2D> m_viewDelegate;

		private Object[] m_objects;
		private Vector2 m_scrollPos = Vector2.zero;
		private float m_ratio = 1.0f;
		public ObjectSelectorWizard ()
		{
		}
		
		public static void ShowWizard<SelectType>( string title, System.Action<Object> selectDelegate, System.Func<Object, Texture2D> viewDelegate, float ratio)
		{
			ObjectSelectorWizard wizard = ScriptableWizard.DisplayWizard<ObjectSelectorWizard>(title);
			wizard.m_delegate = selectDelegate;
			wizard.m_viewDelegate = viewDelegate;
			wizard.m_ratio = ratio;
			wizard.minSize = wizard.maxSize = new Vector2( 570 * ratio, 550 * ratio );
			wizard.m_objects = Resources.FindObjectsOfTypeAll(typeof(SelectType));
		}
		
		void OnWizardUpdate()
		{
			
		}
		
		void OnGUI()
		{
			float padding = 5 * m_ratio;
			float itemSize = 50 * m_ratio;
			float itemPadding = padding + itemSize;

			Rect position = new Rect(padding, padding, itemSize, itemSize );

			int objectLength = m_objects.Length;
			float objectHeight = ( ( objectLength / 10 ) + 1 ) * itemPadding;
			m_scrollPos = GUI.BeginScrollView(new Rect(0, 0, 570 * m_ratio, 550 * m_ratio), m_scrollPos, new Rect(padding, padding, 550 * m_ratio, objectHeight ));
			for( int i = 0; i < objectLength; ++i )
			{
				Texture2D buttonImage = null;
				if( null != m_viewDelegate )
				{
					buttonImage = m_viewDelegate(m_objects[i]);
				}

				bool isPress = false;
				if( null == buttonImage )
				{
					string name = (m_objects[i] as Object).name;
					isPress = GUI.Button (position, name );
				}
				else
				{
					isPress = GUI.Button (position, new GUIContent( buttonImage) );
				}

				if( isPress )
				{
					m_delegate(m_objects[i]);
					
					Close ();
				}

				if( (i + 1) % 10 == 0 )
				{
					position.x = padding;
					position.y += itemPadding;
				}
				else
				{
					position.x += itemPadding;
				}
			}
			
			GUI.EndScrollView();
			
		}
		
		void Collect()
		{
		}
	}
}

