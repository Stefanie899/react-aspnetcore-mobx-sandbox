import { createTypedHooks } from 'easy-peasy';
import AuthStore from 'interfaces/stores/auth-store';

const typedHooks = createTypedHooks<AuthStore>();

export const useAuthStoreActions  = typedHooks.useStoreActions;
export const useAuthStoreDispatch = typedHooks.useStoreDispatch;
export const useAuthStoreState    = typedHooks.useStoreState;