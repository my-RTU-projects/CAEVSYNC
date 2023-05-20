import React, {createContext, useContext, useEffect} from "react";
import axios from "../../utils/axios";
import {enqueueSnackbar} from "notistack";
import {AuthContext} from "./AuthProvider";
import {ConnectedAccountModel} from "../../models/ConnectedAccountModel";
import {useAtomValue, useSetAtom} from "jotai";
import {
    calendarListAtom,
    calendarListForSelectAtom,
    connectedAccountListAtom, isSyncJobActiveAtom, selectedAccountIdAtom, selectedCalendarIdAtom,
    selectedSyncRuleAtom, selectedSyncRuleIdAtom, sourceCalendarRenderModelAtom,
    syncRuleListAtom, targetCalendarRenderModelAtom
} from "./atoms";
import {CalendarSelectModel} from "../../models/CalendarSelectModel";
import {SyncRuleListModel} from "../../models/SyncRuleListModel";
import {SyncRuleModel} from "../../models/SyncRuleModel";
import {SyncRuleCreateModel} from "../../models/SyncRuleCreateModel";
import {SyncRuleEditModel} from "../../models/SyncRuleEditModel";
import {CalendarRenderModel} from "../../models/CalendarRenderModel";

interface DataQueryContextState {
    getConnectedAccountList: () => void;
    deleteConnectedAccount: (accountId: string) => void;
    getCalendarList: () => void;
    getCalendarListForSelect: () => void;
    getSyncRuleList: () => void;
    getSyncRule: () => void;
    createSyncRule: (model: SyncRuleCreateModel) => void;
    editSyncRule: (model: SyncRuleEditModel) => void;
    deleteSyncRule: (syncRuleId: number) => void;
    addMicrosoftAccount: () => void;
    addGoogleAccount: () => void;
    getSourceCalendarRenderModel: () => void;
    getTargetCalendarRenderModel: () => void;
    startSynchronization: () => void;
}

interface DataQueryProviderProps {
    children: React.ReactNode;
}

export const DataQueryContext = createContext<DataQueryContextState>({} as DataQueryContextState);

const DataQueryProvider: React.FC<DataQueryProviderProps> = ({ children }: DataQueryProviderProps) => {
    const authContext = useContext(AuthContext);

    const connectedAccountList = useAtomValue(connectedAccountListAtom);
    const selectedAccountId = useAtomValue(selectedAccountIdAtom);
    const selectedCalendarId = useAtomValue(selectedCalendarIdAtom);
    const selectedSyncRuleId = useAtomValue(selectedSyncRuleIdAtom);
    const selectedSyncRule = useAtomValue(selectedSyncRuleAtom);

    const setConnectedAccountList = useSetAtom(connectedAccountListAtom);
    const setCalendarList = useSetAtom(calendarListAtom);
    const setCalendarListForSelect = useSetAtom(calendarListForSelectAtom);
    const setSyncRuleList = useSetAtom(syncRuleListAtom);
    const setSelectedSyncRuleId = useSetAtom(selectedSyncRuleIdAtom);
    const setSelectedSyncRule = useSetAtom(selectedSyncRuleAtom);
    const setSourceCalendarRenderModel = useSetAtom(sourceCalendarRenderModelAtom);
    const setTargetCalendarRenderModel = useSetAtom(targetCalendarRenderModelAtom);
    const setIsSyncJobActive = useSetAtom(isSyncJobActiveAtom);
    const setSelectedCalendarId = useSetAtom(selectedCalendarIdAtom);
    const setSelectedAccountId = useSetAtom(selectedAccountIdAtom);

    // Connected accounts queries
    const getConnectedAccountList = () => {
        axios
            .get<Array<ConnectedAccountModel>>(
                `connectedAccounts/list`,
                {
                    headers: { Authorization: `Bearer ${authContext.authData?.token}` }
                })
            .then(response => {
                setConnectedAccountList(response.data);
            })
            .catch(reason => enqueueSnackbar(reason.response.data, {variant: "error"}));
    }

    const deleteConnectedAccount = (accountId: string) => {
        axios
            .delete(
                `connectedAccounts/${accountId}`,
                {
                    headers: { Authorization: `Bearer ${authContext.authData?.token}` }
                })
            .then(response => {
                if (selectedAccountId === accountId)
                    setSelectedAccountId(undefined);
                enqueueSnackbar("Konts veiksmīgi izdzēsts", {variant: "success"});
                getConnectedAccountList();
            })
            .catch(reason => enqueueSnackbar(reason.response.data, {variant: "error"}));
    }

    // Calendar queries
    const getCalendarList = () => {
        if (!selectedAccountId) {
            setCalendarList([]);
            return;
        }

        axios
            .get<Array<CalendarSelectModel>>(
                `calendar/list/${selectedAccountId}`,
                {
                    headers: { Authorization: `Bearer ${authContext.authData?.token}` }
                })
            .then(response => {
                if (response.data.every(x => x.calendarIdByProvider !== selectedCalendarId))
                    setSelectedCalendarId(undefined);
                setCalendarList(response.data);
            })
            .catch(reason => enqueueSnackbar(reason.response.data, {variant: "error"}));
    }

    const getCalendarListForSelect = () => {
        axios
            .get<Array<CalendarSelectModel>>(
                `calendar/list/all`,
                {
                    headers: { Authorization: `Bearer ${authContext.authData?.token}` }
                })
            .then(response => {
                setCalendarListForSelect(response.data);
            })
            .catch(reason => enqueueSnackbar(reason.response.data, {variant: "error"}));
    }

    const getSourceCalendarRenderModel = () => {
        if (!selectedAccountId || !selectedSyncRule) {
            setSourceCalendarRenderModel(undefined);
            return;
        }

        axios
            .get<CalendarRenderModel>(
                `calendar/renderModel/${encodeURI(selectedSyncRule?.originCalendarId)}`,
                {
                    headers: { Authorization: `Bearer ${authContext.authData?.token}` }
                })
            .then(response => setSourceCalendarRenderModel(response.data))
            .catch(reason => enqueueSnackbar(reason.response.data, {variant: "error"}));
    }

    const getTargetCalendarRenderModel = () => {
        if (!selectedAccountId || !selectedSyncRule) {
            setTargetCalendarRenderModel(undefined);
            return;
        }

        axios
            .get<CalendarRenderModel>(
                `calendar/renderModel/${encodeURI(selectedSyncRule?.targetCalendarId)}`,
                {
                    headers: { Authorization: `Bearer ${authContext.authData?.token}` }
                })
            .then(response => setTargetCalendarRenderModel(response.data))
            .catch(reason => enqueueSnackbar(reason.response.data, {variant: "error"}));
    }

    // Sync rule queries
    const getSyncRuleList = () => {
        if (!selectedCalendarId) {
            setSyncRuleList([]);
            return;
        }

        axios
            .get<Array<SyncRuleListModel>>(
                `syncRules/list/${selectedCalendarId}`,
                {
                    headers: { Authorization: `Bearer ${authContext.authData?.token}` }
                })
            .then(response => {
                if (response.data.every(x => x.id !== selectedSyncRuleId))
                    setSelectedSyncRuleId(undefined);
                setSyncRuleList(response.data);
            })
            .catch(reason => enqueueSnackbar(reason.response.data, {variant: "error"}));
    }

    const getSyncRule = () => {
        if (!selectedSyncRuleId) {
            setSelectedSyncRule(undefined);
            return;
        }

        axios
            .get<SyncRuleModel>(
                `syncRules/${selectedSyncRuleId}`,
                {
                    headers: { Authorization: `Bearer ${authContext.authData?.token}` }
                })
            .then(response => {
                setSelectedSyncRule(response.data);
            })
            .catch(reason => enqueueSnackbar(reason.response.data, {variant: "error"}));
    }

    const createSyncRule = (model: SyncRuleCreateModel) => {
        axios
            .post<number>(
                `syncRules/create`,
                model,
                {
                    headers: { Authorization: `Bearer ${authContext.authData?.token}` }
                })
            .then(response => {
                getSyncRuleList();
                enqueueSnackbar("Izveidota jauna sinhronizācijas kārtula", {variant: "success"})
            })
            .catch(reason => enqueueSnackbar(reason.response.data, {variant: "error"}));
    }

    const editSyncRule = (model: SyncRuleEditModel) => {
        axios
            .post(
                `syncRules/edit`,
                model,
                {
                    headers: { Authorization: `Bearer ${authContext.authData?.token}` }
                })
            .then(response => {
                getSyncRuleList();
                enqueueSnackbar("Sinhronizācijas kārtula veiksmīgi atjaunota", {variant: "success"})
            })
            .catch(reason => enqueueSnackbar(reason.response.data, {variant: "error"}));
    }

    const deleteSyncRule = (syncRuleId: number) => {
        axios
            .delete(
                `syncRules/delete/${syncRuleId}`,
                {
                    headers: { Authorization: `Bearer ${authContext.authData?.token}` }
                })
            .then(response => {
                if (syncRuleId === selectedSyncRuleId) {
                    setSelectedSyncRuleId(undefined);
                    setSelectedSyncRule(undefined);
                }
                getSyncRuleList();
                enqueueSnackbar("Sinhronizācijas kārtula veiksmīgi dzēsta", {variant: "success"})
            })
            .catch(reason => enqueueSnackbar(reason.response.data, {variant: "error"}));
    }

    const addMicrosoftAccount = () => {
        axios
            .get<string>(
                "microsoftAuth/authCodeRequest",
                {
                    headers: { Authorization: `Bearer ${authContext.authData?.token}` }
                })
            .then(response => window.open(response.data, "_self"))
            .catch(reason => enqueueSnackbar(reason.response.data, {variant: "error"}));
    }

    const addGoogleAccount = () => {
        axios
            .get<string>(
                "googleAuth/authCodeRequest",
                {
                    headers: { Authorization: `Bearer ${authContext.authData?.token}` }
                })
            .then(response => window.open(response.data, "_self"))
            .catch(reason => enqueueSnackbar(reason.response.data, {variant: "error"}));
    }

    const startSynchronization = () => {
        axios
            .get(
                "calendarSync",
                {
                    headers: { Authorization: `Bearer ${authContext.authData?.token}` }
                })
            .then(response => {})
            .catch(reason => enqueueSnackbar(reason.response.data, {variant: "error"}));
    }

    const checkIsSyncJobActive = () => {
        axios
            .get(
                "calendarSync/isActive",
                {
                    headers: { Authorization: `Bearer ${authContext.authData?.token}` }
                })
            .then(response => setIsSyncJobActive(response.data))
            .catch(reason => enqueueSnackbar(reason.response.data, {variant: "error"}));
    }

    useEffect(() => {
        getConnectedAccountList();
    }, [])

    useEffect(() => {
        getCalendarList();
        getCalendarListForSelect();
    }, [selectedAccountId])

    useEffect(() => {
        getSyncRuleList();
    }, [selectedCalendarId])

    useEffect(() => {
        getSyncRule();
    }, [selectedSyncRuleId])

    useEffect(() => {
        getSourceCalendarRenderModel();
        getTargetCalendarRenderModel();
    }, [selectedSyncRule])

    useEffect(() => {
        checkIsSyncJobActive();
        // refresh data every 5 sec
        setInterval(() => {
            checkIsSyncJobActive();
        }, 5000);
    }, [])

    return (
        <DataQueryContext.Provider
            value={{
                getConnectedAccountList: getConnectedAccountList,
                deleteConnectedAccount: deleteConnectedAccount,
                getCalendarList: getCalendarList,
                getCalendarListForSelect: getCalendarListForSelect,
                getSyncRuleList: getSyncRuleList,
                getSyncRule: getSyncRule,
                createSyncRule: createSyncRule,
                editSyncRule: editSyncRule,
                deleteSyncRule: deleteSyncRule,
                addMicrosoftAccount: addMicrosoftAccount,
                addGoogleAccount: addGoogleAccount,
                getSourceCalendarRenderModel: getSourceCalendarRenderModel,
                getTargetCalendarRenderModel: getTargetCalendarRenderModel,
                startSynchronization: startSynchronization
            }}
        >
            {children}
        </DataQueryContext.Provider>
    );
}

export default DataQueryProvider;