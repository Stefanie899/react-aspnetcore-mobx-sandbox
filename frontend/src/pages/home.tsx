import React from "react";

const HomePage: React.FC = () => {
    return (
        <React.Fragment>
            <div>
                <h1>Topic to Updoot</h1>
                <p>
                    This is the content of the topic to updoot.
                </p>
                <a href="#">Updoot</a>
                <a href="#">Downdoot</a>
                <a href="#">Comment</a>
            </div>
        </React.Fragment>
    );
}

export default HomePage;