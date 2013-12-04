using System;
using IntrospectionCacheData = NVelocity.Util.Introspection.IntrospectionCacheData;

namespace NVelocity.Context {
	
    /// <summary>  interface to bring all necessary internal and user contexts together.
    /// this is what the AST expects to deal with.  If anything new comes
    /// along, add it here.
    /// *
    /// I will rename soon :)
    /// *
    /// </summary>
    /// <author> <a href="mailto:geirm@optonline.net">Geir Magnusson Jr.</a>
    /// </author>
    /// <version> $Id: InternalContextAdapter.cs,v 1.2 2002/07/30 07:26:49 corts Exp $
    /// 
    /// </version>
	
    public interface InternalContextAdapter : InternalHousekeepingContext, IContext, InternalWrapperContext, InternalEventContext {
    }
}