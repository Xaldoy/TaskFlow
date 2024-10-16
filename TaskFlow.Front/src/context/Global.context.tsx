// UserContext.tsx
import {
    createContext,
    useState,
    Dispatch,
    SetStateAction,
    useContext,
    ReactNode,
} from "react";
import AuthResult from "../models/AuthResult";

interface GlobalContextProps {
    userIsLoggedIn: boolean | undefined;
    setIsUserLoggedIn: Dispatch<SetStateAction<boolean | undefined>>;
    loggedInUser: AuthResult | undefined;
    setLoggedInUser: Dispatch<SetStateAction<AuthResult | undefined>>;
    pageIsLoading: boolean;
    setPageIsLoading: Dispatch<SetStateAction<boolean>>;
}

const GlobalContext = createContext<GlobalContextProps>({
    loggedInUser: undefined,
    setIsUserLoggedIn: () => { },
    userIsLoggedIn: undefined,
    setLoggedInUser: () => { },
    pageIsLoading: false,
    setPageIsLoading: () => { },
});

interface ProviderProps {
    children: ReactNode;
}

export const useGlobalContext = () => {
    return useContext(GlobalContext);
};

export function GlobalContextProvider({ children }: ProviderProps) {
    const [loggedInUser, setLoggedInUser] = useState<AuthResult>();
    const [pageIsLoading, setPageIsLoading] = useState(false);
    const [userIsLoggedIn, setIsUserLoggedIn] = useState<boolean>();
    return (
        <GlobalContext.Provider
            value={{
                userIsLoggedIn,
                setIsUserLoggedIn,
                loggedInUser,
                setLoggedInUser,
                pageIsLoading,
                setPageIsLoading,
            }}
        >
            {children}
        </GlobalContext.Provider>
    );
}
