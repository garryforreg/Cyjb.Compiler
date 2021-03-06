﻿using System.Diagnostics;
using System.Text;

namespace Cyjb.Compiler.RegularExpressions
{
	/// <summary>
	/// 表示定位点的正则表达式。
	/// </summary>
	public sealed class AnchorExp : Regex
	{
		/// <summary>
		/// 表示行的结尾的正则表达式。
		/// </summary>
		internal static new readonly Regex EndOfLine = Regex.Symbol('\r').Optional().Concat(Regex.Symbol('\n'));
		/// <summary>
		/// 内部正则表达式。
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly Regex innerExp;
		/// <summary>
		/// 使用内部正则表达式初始化 <see cref="AnchorExp"/> 类的新实例。
		/// </summary>
		/// <param name="innerExp">包含的内部正则表达式。</param>
		internal AnchorExp(Regex innerExp)
		{
			ExceptionHelper.CheckArgumentNull(innerExp, "innerExp");
			CheckRegex(innerExp);
			this.innerExp = innerExp;
			this.BeginningOfLine = false;
			this.TrailingExpression = null;
		}
		/// <summary>
		/// 获取或设置是否定位到行的起始。
		/// </summary>
		public new bool BeginningOfLine { get; set; }
		/// <summary>
		/// 获取内部的正则表达式。
		/// </summary>
		public Regex InnerExpression { get { return innerExp; } }
		/// <summary>
		/// 获取或设置要向前看的正则表达式。
		/// </summary>
		public Regex TrailingExpression { get; set; }
		///// <summary>
		///// 获取向前看的正则表达式的 NFA 状态。
		///// </summary>
		//internal NfaState TrailingHeadState { get; private set; }
		///// <summary>
		///// 根据当前的正则表达式构造 NFA。
		///// </summary>
		///// <param name="nfa">要构造的 NFA。</param>
		//internal override void BuildNfa(Nfa nfa)
		//{
		//	innerExp.BuildNfa(nfa);
		//	if (TrailingExpression != null)
		//	{
		//		NfaState head = nfa.HeadState;
		//		TrailingHeadState = nfa.TailState;
		//		TrailingExpression.BuildNfa(nfa);
		//		TrailingHeadState.Add(nfa.HeadState);
		//		nfa.HeadState = head;
		//	}
		//}
		/// <summary>
		/// 获取当前正则表达式匹配的字符长度。变长度则为 <c>-1</c>。
		/// </summary>
		public override int Length
		{
			get { return innerExp.Length; }
		}
		/// <summary>
		/// 返回当前对象的字符串表示形式。
		/// </summary>
		/// <returns>当前对象的字符串表示形式。</returns>
		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			if (BeginningOfLine)
			{
				builder.Append('^');
			}
			builder.Append(innerExp.ToString());
			if (TrailingExpression != null)
			{
				if (TrailingExpression == EndOfLine)
				{
					builder.Append('$');
				}
				else
				{
					builder.Append('/');
					builder.Append(TrailingExpression.ToString());
				}
			}
			return builder.ToString();
		}
	}
}
