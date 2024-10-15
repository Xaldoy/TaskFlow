import { observer } from "mobx-react";
import useAuth from "../../../hooks/useAuth";
import { ChangeEvent, FormEvent, useState } from "react";
import RegisterAttempt from "../../../models/RegisterAttempt";
import errorStore from "../../../stores/ErrorStore";
import { ErrorTypes } from "../../../enums/ErrorTypes";
import AuthResult from "../../../models/AuthResult";
import { TextInputField, Button } from "evergreen-ui";

const RegisterForm = observer(() => {

    const auth = useAuth();

    const [registerAttempt, setRegisterAttempt] = useState<RegisterAttempt>(
        {
            email: "",
            username: "",
            password: ""
        }
    )

    const handleRegister = async (registerAttempt: RegisterAttempt) => {
        errorStore.removeErrors(ErrorTypes.AuthenticationError);
        const authResult: AuthResult = await auth.register(registerAttempt);
        if (authResult.token) {
            alert("Registered");
        }
    }

    const handleFormSubmit = (e: FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        handleRegister(registerAttempt);
    }

    return <form onSubmit={handleFormSubmit}>
        <TextInputField
            label="Username"
            name="username"
            value={registerAttempt.username}
            onChange={(e: ChangeEvent<HTMLInputElement>) => setRegisterAttempt((prev: RegisterAttempt) => ({
                ...prev,
                username: e.target.value
            }))} />
        <TextInputField
            label="Email"
            name="email"
            value={registerAttempt.email}
            onChange={(e: ChangeEvent<HTMLInputElement>) => setRegisterAttempt((prev: RegisterAttempt) => ({
                ...prev,
                email: e.target.value
            }))} />
        <TextInputField
            label="Password"
            name="password"
            type="password"
            value={registerAttempt.password}
            onChange={(e: ChangeEvent<HTMLInputElement>) => setRegisterAttempt((prev: RegisterAttempt) => ({
                ...prev,
                password: e.target.value
            }))} />
        <Button type="submit">Register</Button>
    </form>

})

export default RegisterForm;