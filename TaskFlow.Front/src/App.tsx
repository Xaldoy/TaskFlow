import { useEffect } from "react";
import "./App.css";
import AuthForm from "./components/auth/AuthForm";
import { GlobalContextProvider } from "./context/Global.context";
import useAuth from "./hooks/useAuth";

function App() {

    const auth = useAuth();

    useEffect(() => {
        auth.handleUserLoggedInCheck();
    }, [])

    return (
        <div className="appContainer">
            <AuthForm />
        </div>
    );
}

export default App;
