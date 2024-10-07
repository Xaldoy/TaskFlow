import { Alert, Button, TextInputField } from "evergreen-ui";
import { ChangeEvent, FormEvent, MouseEventHandler, useState } from "react";
import LoginAttempt from "../../models/LoginAttempt";
import useAuth from "../../hooks/useAuth";
import AuthResult from "../../models/AuthResult";
import { observer } from "mobx-react";
import errorStore from "../../stores/ErrorStore";
import localStyles from "./AuthForm.module.css"
import { ErrorTypes } from "../../enums/ErrorTypes";

const AuthForm = observer(() => {

    const auth = useAuth();

    const [loginAttempt, setLoginAttempt] = useState<LoginAttempt>(
        {
            credentials: "",
            password: ""
        }
    );

    const authErrors = errorStore.getAuthenticationErrors();

    const handleLogin = async (loginAttempt: LoginAttempt) => {
        errorStore.removeErrors(ErrorTypes.AuthenticationError);
        const authResult: AuthResult = await auth.login(loginAttempt);
        if (authResult.token) {
            alert("Correct")
        }
    }

    const handleFormSubmit = (e: FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        handleLogin(loginAttempt);
    }

    return <>
        <div className={localStyles.authFormContainer}>
            <form onSubmit={handleFormSubmit}>
                <TextInputField
                    label="Credentials"
                    name="credentials"
                    value={loginAttempt.credentials}
                    onChange={(e: ChangeEvent<HTMLInputElement>) => setLoginAttempt((prev: LoginAttempt) => ({
                        ...prev,
                        credentials: e.target.value
                    }))} />
                <TextInputField
                    label="Password"
                    name="password"
                    type="password"
                    value={loginAttempt.password}
                    onChange={(e: ChangeEvent<HTMLInputElement>) => setLoginAttempt((prev: LoginAttempt) => ({
                        ...prev,
                        password: e.target.value
                    }))} />
                <Button type="submit">Login</Button>
            </form>
            {authErrors.length > 0 && (
                <Alert intent="danger" >{authErrors[0].errorMessage}</Alert>
            )}
        </div>
    </>
})

export default AuthForm;