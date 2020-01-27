import React from 'react'

export const useStores = <T>(context: React.Context<T>) => React.useContext(context);