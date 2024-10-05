import { useState } from "react";
import "./App.css";
import LoginAttempt from "./models/LoginAttempt";
import useAuth from "./hooks/useAuth";

function App() {
    const [loginAttempt, setLoginAttempt] = useState<LoginAttempt>({
        credentials: "",
        password: "",
    });

    const auth = useAuth();

    return (
        <>
            <input
                onChange={(e) =>
                    setLoginAttempt((prev: LoginAttempt) => ({
                        ...prev,
                        credentials: e.target.value,
                    }))
                }
            />
            <input
                onChange={(e) =>
                    setLoginAttempt((prev: LoginAttempt) => ({
                        ...prev,
                        password: e.target.value,
                    }))
                }
            />
            <button onClick={() => auth.login(loginAttempt)}>Login</button>
        </>
    );
}

export default App;
