import React, { useState, useEffect } from "react";

interface ButtonProps {
    isLoading?:      boolean;
    onClick?:        () => void;
    loadingOnClick?: boolean;
    text:            string;
}

const Button: React.FC<ButtonProps> = (props: ButtonProps) => {
    const [isLoading, setLoading] = useState(false);

    const onClick = () => {
        if (isLoading) {
            return;
        }

        if (props.loadingOnClick) {
            setLoading(true);
        }

        if (props.onClick != null) {
            props.onClick();
        }
    }

    useEffect(() => {
        setLoading(props.isLoading == null ? false : props.isLoading);
    }, [props.isLoading])

    return (
        <button onClick = {onClick}>
            { isLoading ? "Loading..." : props.text }
        </button>
    )
}

Button.defaultProps = { loadingOnClick: true }

export default Button;