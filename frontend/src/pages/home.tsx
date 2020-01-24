import React, { useState, useEffect } from "react";
import TopicsServices from "services/topics/topics-service";
import Topic from "interfaces/topics/topic";
import TopicCard from "molecules/topic-card";

const HomePage: React.FC = () => {
    const [topics, setTopics] = useState(Array<Topic>());

    useEffect(() => {
        const fetchData = async () => {
            const topics = await TopicsServices.get();

            setTopics(topics.resultObject);
        }

        fetchData();
    }, [])

    return (
        <React.Fragment>
            {
                topics.map((t) => {
                    return (
                        <TopicCard
                            topic = {t} />
                    )
                })
            }
        </React.Fragment>
    );
}

export default HomePage;