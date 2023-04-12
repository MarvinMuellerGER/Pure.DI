// ReSharper disable InconsistentNaming
namespace Pure.DI.Core.Models;

internal enum Setting
{
    /// <summary>
    /// Determine whether to generate <c>Resolve</c> methods. <c>On</c> or <c>Off</c>. <c>On</c> by default.
    /// </summary>
    Resolve,
    
    /// <summary>
    /// Determine whether to generate <c>OnInstanceCreation</c> method.<c>On</c> or <c>Off</c>. <c>On</c> by default.
    /// </summary>
    OnInstanceCreation,
    
    /// <summary>
    /// The regular expression to filter OnInstanceCreation by the instance type name. ".+" by default.
    /// </summary>
    OnInstanceCreationImplementationTypeNameRegularExpression,
    
    /// <summary>
    /// The regular expression to filter OnInstanceCreation by the tag. ".+" by default.
    /// ".+" by default. 
    /// </summary>
    OnInstanceCreationTagRegularExpression,
    
    /// <summary>
    /// The regular expression to filter OnInstanceCreation by the lifetime. ".+" by default.
    /// ".+" by default. 
    /// </summary>
    OnInstanceCreationLifetimeRegularExpression,
    
    /// <summary>
    /// Determine whether to generate partial <c>OnDependencyInjection</c> method to control of dependency injection. <c>On</c> or <c>Off</c>. <c>Off</c> by default.
    /// </summary>
    OnDependencyInjection,
    
    /// <summary>
    /// The regular expression to filter OnDependencyInjection by the instance type name. ".+" by default.
    /// </summary>
    OnDependencyInjectionImplementationTypeNameRegularExpression,
    
    /// <summary>
    /// The regular expression to filter OnDependencyInjection by the resolving type name. ".+" by default.
    /// </summary>
    OnDependencyInjectionContractTypeNameRegularExpression,
    
    /// <summary>
    /// The regular expression to filter OnDependencyInjection by the tag. ".+" by default.
    /// ".+" by default. 
    /// </summary>
    OnDependencyInjectionTagRegularExpression,
    
    /// <summary>
    /// The regular expression to filter OnDependencyInjection by the lifetime. ".+" by default.
    /// ".+" by default. 
    /// </summary>
    OnDependencyInjectionLifetimeRegularExpression,
    
    /// <summary>
    /// Determine whether to generate partial <c>OnCannotResolve</c> method to control of dependency injection. <c>On</c> or <c>Off</c>. <c>Off</c> by default.
    /// </summary>
    OnCannotResolve,
    
    /// <summary>
    /// The regular expression to filter OnCannotResolve by the resolving type name. ".+" by default.
    /// </summary>
    OnCannotResolveContractTypeNameRegularExpression,
    
    /// <summary>
    /// The regular expression to filter OnCannotResolve by the tag. ".+" by default.
    /// ".+" by default. 
    /// </summary>
    OnCannotResolveTagRegularExpression,
    
    /// <summary>
    /// The regular expression to filter OnCannotResolve by the lifetime. ".+" by default.
    /// ".+" by default. 
    /// </summary>
    OnCannotResolveLifetimeRegularExpression,
    
    /// <summary>
    /// <c>On</c> or <c>Off</c>.
    /// <c>Off</c> by default. 
    /// </summary>
    ToString,
    
    /// <summary>
    /// <c>On or <c>Off</c>.
    /// <c>On</c> by default. 
    /// </summary>
    ThreadSafe
}