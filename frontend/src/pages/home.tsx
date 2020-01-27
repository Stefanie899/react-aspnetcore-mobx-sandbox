import React, { useState, useEffect } from "react";
import TopicsServices from "services/topics/topics-service";
import Topic from "interfaces/topics/topic";
import TopicCard from "molecules/topic-card";
import TopicDoot from "interfaces/topics/topic-doot";
import TopicDootsService from "services/topics/topic-doots-service";

const HomePage: React.FC = () => {
    const [topics, setTopics]         = useState(Array<Topic>());
    const [topicDoots, setTopicDoots] = useState(Array<TopicDoot>());
    const [refresh, setRefresh]       = useState(true);

    useEffect(() => {
        const fetchData = async () => {
            const topics = await TopicsServices.get();

            setTopics(topics.resultObject);
        }

        const fetchDoots = async () => {
            const topics = await TopicDootsService.get();

            setTopicDoots(topics.resultObject);
        }

        fetchData();
        fetchDoots();

        setRefresh(false);
    }, [refresh])

    return (
        <React.Fragment>
            {
                topics.map((t) => {
                    const doot = topicDoots?.find(e => e.userId == 1 && e.topicId == t.id);

                    return (
                        <TopicCard
                            key       = {t.id}
                            onDoot    = {() => setRefresh(true)}
                            topic     = {t}
                            topicDoot = {doot} />
                    )
                })
            }
        </React.Fragment>
    );
}

export default HomePage;