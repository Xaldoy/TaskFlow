import { Navigate } from "react-router-dom";
import { useGlobalContext } from "../../../context/Global.context";

interface PrivateRouteProps {
    page: JSX.Element;
}

export default function PublicRoute(props: PrivateRouteProps) {

    const globalContext = useGlobalContext();

    const isLoggedIn = globalContext.userIsLoggedIn;

    if (isLoggedIn === undefined) return <></>

    return isLoggedIn === false ? props.page : <Navigate to="/dashboard" />
}