import { useSelector } from 'react-redux';
import translation from './translation.json';

function Translation(props) {
    const lang = useSelector((state) => state.user.lang)

    function getLang() {
        try {
            if (lang === 'TR') {
            return translation[props.msg].TR
        } else if (lang === 'ENG'){
            return translation[props.msg].ENG
        } else {
            return translation[props.msg].TR
        }
        } catch (error) {
            console.error(error)
            console.info('translation msg : ' + props.msg)
        }
        
    }
    return (
        getLang()
    )
}

export default Translation;