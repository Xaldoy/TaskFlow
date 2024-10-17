import { useEffect } from "react";
import "./App.css";
import AuthForm from "./components/auth/AuthForm";
import useAuth from "./hooks/useAuth";
import { useGlobalContext } from "./context/Global.context";
import AuthResult from "./models/AuthResult";
import { BrowserRouter, Navigate, Route, Routes } from "react-router-dom";
import PrivateRoute from "./components/auth/routing/PrivateRoute";
import Dashboard from "./components/dashboard/Dashboard";
import PublicRoute from "./components/auth/routing/PublicRoute";

function App() {

    const auth = useAuth();

    useEffect(() => {
        auth.handleUserLoggedInCheck();
    }, [])

    return (
        <BrowserRouter>
            <div className="appContainer">
                <Routes>
                    <Route path="dashboard" element={<PrivateRoute page={<Dashboard />} />} />
                    <Route path="login" element={<PublicRoute page={<AuthForm />} />} />
                    <Route path="*" element={<Navigate to={"/login"} />} />
                </Routes>
            </div>
        </BrowserRouter>
    );
}

export default App;
