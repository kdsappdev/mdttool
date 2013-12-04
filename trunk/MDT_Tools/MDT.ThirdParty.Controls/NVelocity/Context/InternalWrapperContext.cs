using System;


namespace NVelocity.Context {
	
    /// <summary>
    /// interface for internal context wrapping functionality
    /// </summary>
    /// <author> <a href="mailto:geirm@optonline.net">Geir Magnusson Jr.</a></author>
    /// <version> $Id: InternalWrapperContext.cs,v 1.3 2002/07/30 08:25:26 corts Exp $ </version>
    public interface InternalWrapperContext {
	/// <summary>
	/// returns the wrapped user context 
	/// </summary>
	IContext InternalUserContext {
	    get;
	}

	/// <summary>
	/// returns the base full context impl 
	/// </summary>
	InternalContextAdapter BaseContext {
	    get;
	}

    }
}