package mono.android.app;

public class ApplicationRegistration {

	public static void registerApplications ()
	{
				// Application and Instrumentation ACWs must be registered first.
		mono.android.Runtime.register ("ScanJect.MainApplication, ScanJect, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", crc64c16b1cd9494a077f.MainApplication.class, crc64c16b1cd9494a077f.MainApplication.__md_methods);
		
	}
}
