import React from "react";
import Topic from "interfaces/topics/topic";
import { useAuthStoreState } from "hooks/auth-store-hooks";

const TopicCard: React.FC<{ topic: Topic }> = (props) => {
    const { topic } = props;

    const authState = useAuthStoreState(actions => actions);

    return (
        <div className="topic-card">
            <h1>{topic.title}</h1>
            <div className = "content">
                <p>
                    {topic.body}
                </p>
            </div>
            <ul>
                { // if
                    authState.isAuthenticated &&
                    <li>
                        <a href="#">Updoot</a>
                    </li>
                }
                <li>
                    {topic.updoots} updoots
                </li>
                { // if
                    authState.isAuthenticated &&
                    <li>
                        <a href="#">Downdoot</a>
                    </li>
                }
                <li>
                    {topic.downdoots} downdoots
                </li>
                { // if
                    authState.isAuthenticated &&
                    <li>
                        <a href="#">Comment</a>
                    </li>
                }
                <li>
                    0 comments
                </li>
            </ul>
        </div>
    );
}

export default TopicCard;