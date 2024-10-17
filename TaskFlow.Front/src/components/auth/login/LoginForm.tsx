import { TextInputField, Button } from "evergreen-ui";
import { ChangeEvent, FormEvent, useState } from "react";
import LoginAttempt from "../../../models/LoginAttempt";
import { observer } from "mobx-react";
import errorStore from "../../../stores/ErrorStore";
import { ErrorTypes } from "../../../enums/ErrorTypes";
import useAuth from "../../../hooks/useAuth";
import AuthResult from "../../../models/AuthResult";
import { useNavigate } from "react-router-dom";

const LoginForm = observer(() => {

    const auth = useAuth();
    const navigate = useNavigate();

    const [loginAttempt, setLoginAttempt] = useState<LoginAttempt>(
        {
            credentials: "",
            password: ""
        }
    );

    const handleLogin = async (loginAttempt: LoginAttempt) => {
        errorStore.removeErrors(ErrorTypes.AuthenticationError);
        const authResult: AuthResult = await auth.login(loginAttempt);
        if (authResult.userName) {
            navigate("/dashboard")
        }
    }

    const handleFormSubmit = (e: FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        handleLogin(loginAttempt);
    }

    return <form onSubmit={handleFormSubmit} >
        <TextInputField
            label="Emal/Username"
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
})

export default LoginForm;