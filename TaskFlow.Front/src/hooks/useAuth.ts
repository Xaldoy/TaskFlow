import { useGlobalContext } from "../context/Global.context";
import AuthResult from "../models/AuthResult";
import LoginAttempt from "../models/LoginAttempt";
import RegisterAttempt from "../models/RegisterAttempt";
import accountController from "../api/controllers/AccountController";

// Utility for localStorage keys
const USERNAME_KEY = "username";

const useAuth = () => {
    const globalContext = useGlobalContext();

    const handleUserLoggedInCheck = () => {
        const userName = localStorage.getItem(USERNAME_KEY);
        if (userName !== null) {
            globalContext.setLoggedInUser({ userName: userName })
        }
    }

    const login = async (loginData: LoginAttempt): Promise<AuthResult> => {
        try {
            const response: AuthResult = await accountController.Login(loginData);
            if (response.userName) {
                setAuthData(response);
                return response;
            }
            return Promise.reject("Login failed");
        } catch (error) {
            return Promise.reject("Login failed");
        }
    };

    const logout = () => {
        clearAuthData();
        globalContext.setLoggedInUser(undefined);
    };

    const register = async (
        registerData: RegisterAttempt
    ): Promise<AuthResult> => {
        try {
            const response: AuthResult = await accountController.Register(registerData);
            if (response.userName) {
                setAuthData(response);
                return response;
            }
            return Promise.reject("Registration failed");
        } catch (error) {
            return Promise.reject("Registration failed");
        }
    };

    const setAuthData = (authResult: AuthResult) => {
        if (authResult?.userName) {
            localStorage.setItem(USERNAME_KEY, authResult.userName);
            globalContext.setLoggedInUser({ userName: authResult.userName });
        }
    };

    const clearAuthData = () => {
        localStorage.removeItem(USERNAME_KEY);
        globalContext.setLoggedInUser(undefined)
    }

    const getCurrentUserName = (): string | null => {
        return localStorage.getItem(USERNAME_KEY);
    };

    const validate = (
        type: "email" | "password",
        value: string
    ): string | undefined => {
        const validators: { [key: string]: () => string | undefined } = {
            email: () => {
                const isEmailFormat = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
                return !isEmailFormat.test(value)
                    ? "Email is invalid."
                    : undefined;
            },
            password: () => {
                const hasDigit = /\d/;
                const hasLowercase = /[a-z]/;
                const hasUppercase = /[A-Z]/;
                const isValidLength = /^.{8,16}$/;

                if (!isValidLength.test(value))
                    return "Password must be between 8 and 16 characters long.";
                if (!hasDigit.test(value))
                    return "Password must contain at least one digit.";
                if (!hasLowercase.test(value))
                    return "Password must contain at least one lowercase letter.";
                if (!hasUppercase.test(value))
                    return "Password must contain at least one uppercase letter.";
            },
        };
        return validators[type]();
    };

    return {
        login,
        logout,
        register,
        getCurrentUserName,
        handleUserLoggedInCheck,
        validateEmail: (email: string) => validate("email", email),
        validatePassword: (password: string) => validate("password", password),
    };
};

export default useAuth;
