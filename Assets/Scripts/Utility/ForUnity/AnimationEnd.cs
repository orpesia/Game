//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using UnityEngine;

namespace UnnamedUtility
{
	public class AnimationEnd : MonoBehaviour
	{
		public delegate void AnimationEnded( GameObject obj );

		private AnimationEnded m_ended;
		private bool m_isStart;

		public AnimationEnd ()
		{
		}

		void Awake()
		{
			animation.Stop ();

			m_isStart = false;
		}

		void Update()
		{

			if( m_isStart && false == animation.isPlaying )
			{
				m_ended( this.gameObject );

				m_isStart = false;
			}
		}

		public void Play( string name, AnimationEnded ended )
		{
			animation.Play( name );

			m_ended = ended;
			m_isStart = true;
		}

	}
}

