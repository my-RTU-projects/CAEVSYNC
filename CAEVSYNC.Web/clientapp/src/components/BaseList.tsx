import {Box, Collapse, IconButton, IconButtonProps, Paper, styled, Typography} from "@mui/material";
import React, {useState} from "react";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";

interface BaseListProps {
    title: string;
    actions?: JSX.Element;
    children: React.ReactNode;
}

interface ExpandMoreProps extends IconButtonProps {
    isExpand: boolean;
}

const ExpandMore = styled((props: ExpandMoreProps) => {
    const { isExpand, ...other } = props;
    return <IconButton {...other} />;
})(({ theme, isExpand }) => ({
    transform: !isExpand ? 'rotate(0deg)' : 'rotate(180deg)',
    marginLeft: 'auto',
    transition: theme.transitions.create('transform', {
        duration: theme.transitions.duration.shortest,
    }),
}));

const BaseList: React.FC<BaseListProps> = ({title, actions, children}: BaseListProps) => {
    const [isExpanded, setExpanded] = useState(true);

    const handleExpandClick = () => {
        setExpanded(!isExpanded);
    };

    return (
        <Box p={1} pl={2} component={Paper}>
            <Box display="flex" alignItems="center">
                <Typography variant="h6">{title}</Typography>
                <Box sx={{ flex: "1 1 auto" }}/>
                {actions && actions}
                <ExpandMore
                    isExpand={isExpanded}
                    aria-expanded={isExpanded}
                    onClick={handleExpandClick}
                    aria-label="show more"
                >
                    <ExpandMoreIcon />
                </ExpandMore>
            </Box>
            <Collapse in={isExpanded} timeout="auto" unmountOnExit>
                {children ? children : <Typography color="gray">Saraksts ir tukšs</Typography>}
            </Collapse>
        </Box>
    );
}

export default BaseList;