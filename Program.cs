using Microsoft.Extensions.Configuration;

namespace GoogleTextToSpeech
{
	internal static class Program
	{
		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{

			// To customize application configuration such as set high DPI settings or default font,
			// see https://aka.ms/applicationconfiguration.
			ApplicationConfiguration.Initialize();

			try
			{
				var builder = new ConfigurationBuilder()
				  .SetBasePath(Directory.GetCurrentDirectory())
				  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

				IConfiguration configuration = builder.Build();

				Application.Run(new Form1(configuration));
			}
			catch (Exception ex)
			{

				MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

			}

		}
	}
}