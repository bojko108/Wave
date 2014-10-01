﻿using System.Reflection;
using System.Reflection.Emit;

namespace System.Windows.Behaviors
{
    /// <summary>
    ///     Generates delegates according to the specified signature on runtime.
    ///     There are limitations of this generator:
    ///     1. It can only hook to Events that have a Delegate of type void (for example EventHandler)
    ///     2. It uses Reflection so it cannot be used in XBAPs because of the security restrictions there are in XBAPs.
    /// </summary>
    public static class BehaviorEventHandler
    {
        #region Public Methods

        /// <summary>
        ///     Creates a delegate with a matching signature of the supplied eventHandlerType
        ///     This method only supports Events that have a delegate of type void
        /// </summary>
        /// <param name="eventHandlerType">The delegate type to wrap. Note that this must always be a void delegate</param>
        /// <param name="methodToInvoke">The method to invoke</param>
        /// <param name="methodInvoker">The object where the method resides</param>
        /// <returns>Returns a delegate with the same signature as eventHandlerType that calls the methodToInvoke inside</returns>
        public static Delegate CreateDelegate(Type eventHandlerType, MethodInfo methodToInvoke, object methodInvoker)
        {
            // Get the eventHandlerType signature
            var eventHandlerInfo = eventHandlerType.GetMethod("Invoke");
            if (eventHandlerInfo.ReturnParameter != null)
            {
                Type returnType = eventHandlerInfo.ReturnParameter.ParameterType;
                if (returnType != typeof (void))
                    throw new ArgumentException("Delegate has a return type. This only supprts event handlers that are void");
            }

            ParameterInfo[] delegateParameters = eventHandlerInfo.GetParameters();

            // Get the list of type of parameters. Please note that we do + 1 because we have to push the object where the method resides i.e methodInvoker parameter
            Type[] hookupParameters = new Type[delegateParameters.Length + 1];
            hookupParameters[0] = methodInvoker.GetType();
            for (int i = 0; i < delegateParameters.Length; i++)
                hookupParameters[i + 1] = delegateParameters[i].ParameterType;

            DynamicMethod handler = new DynamicMethod("", null,
                hookupParameters, typeof (BehaviorEventHandler));

            ILGenerator eventIL = handler.GetILGenerator();

            // Load the parameters or everything will just BAM :)
            LocalBuilder local = eventIL.DeclareLocal(typeof (object[]));
            eventIL.Emit(OpCodes.Ldc_I4, delegateParameters.Length + 1);
            eventIL.Emit(OpCodes.Newarr, typeof (object));
            eventIL.Emit(OpCodes.Stloc, local);

            // Start from 1 because the first item is the instance. Load up all the arguments
            for (int i = 1; i < delegateParameters.Length + 1; i++)
            {
                eventIL.Emit(OpCodes.Ldloc, local);
                eventIL.Emit(OpCodes.Ldc_I4, i);
                eventIL.Emit(OpCodes.Ldarg, i);
                eventIL.Emit(OpCodes.Stelem_Ref);
            }

            eventIL.Emit(OpCodes.Ldloc, local);

            // Load as first argument the instance of the object for the methodToInvoke i.e methodInvoker
            eventIL.Emit(OpCodes.Ldarg_0);

            // Now that we have it all set up call the actual method that we want to call for the binding
            eventIL.EmitCall(OpCodes.Call, methodToInvoke, null);

            eventIL.Emit(OpCodes.Pop);
            eventIL.Emit(OpCodes.Ret);

            // Create a delegate from the dynamic method
            return handler.CreateDelegate(eventHandlerType, methodInvoker);
        }

        #endregion
    }
}