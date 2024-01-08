using Microsoft.VisualStudio.Shell;
using System.ComponentModel;

namespace Forces
{
	internal class Preferences : DialogPage
	{
		private int _previewMultiSampling = 8;

		[DisplayName("Preview Multisampling")]
		[Description("Multisample anti-aliasing sample count for the Forces preview window.")]
		public int PreviewMultiSampling
		{
			get => _previewMultiSampling;
			set => _previewMultiSampling = value;
		}
	}
}
